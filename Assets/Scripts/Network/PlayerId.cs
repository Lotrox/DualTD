using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PlayerId : NetworkBehaviour {

	static bool DEBUG = true;

	// ATRIBUTOS DE UN JUGADOR:
	// - GameObject de él mismo.
	// - Identificador dentro del juego asignado por NetworkMan.
	// - Color asignado por NetworkMan.
	// - Vida al inicio.
	// - Dinero al inicio.

	[SyncVar(hook = "hook_id")]
	int id;

	[SyncVar(hook = "hook_color")]
	Color c;

	[SyncVar(hook = "hook_health")]
	public int health = 100;

	[SyncVar(hook = "hook_money")]
	public int money = 50;

	// Asignación de la cámara al inicio de la partida.
	// Para la asignación, se considera el Id del jugador.
	public void Start()
	{
		if (!isLocalPlayer)
			return;

		if (id == 0)
		{
			Camera.main.transform.position = new Vector3 (33.0f, 30.0f, -7.0f);
			Camera.main.GetComponent<freeCam>().updateCamera (false);
			GetComponent<Flicking> ().swapMaterial (true); 
		} 
		else if (id == 1) 
		{
			Camera.main.transform.position = new Vector3 (67.0f, 30.0f, 107.0f);
			Camera.main.GetComponent<freeCam>().updateCamera (true);
			GetComponent<Flicking> ().swapMaterial (false); 
		}
	}

	// Establece los atributos principales del jugador: id y color.
	public void setAttributes(int _id, Color _c)
	{
		id = _id;
		c = _c;
	}

	// Devuelve la Id asignada al jugador por NetworkMan.
	public int getId()
	{
		return id;
	}

	// Devuelve el color asignado al jugador por NetworkMan.
	public Color getColor()
	{
		return c;
	}

	// EJECUCIÓN EN EL SERVIDOR.
	// El jugador recibe una cantidad de daño.
	// Si la cantidad (amount) es positiva, pierde vida.
	// Si la cantidad (amount) es negativa, gana vida.
	public void TakeDamage(int amount)
	{
		if (!isServer)
			return;

		health -= amount;
	}

	// EJECUCIÓN EN EL SERVIDOR.
	// Indica si el jugador está muerto. (Su vida es igual o inferior a 0).
	public bool IsDead()
	{
		if (!isServer)
			return false;

		return (health <= 0);
	}

	// EJECUCIÓN EN EL SERVIDOR.
	// El jugador incrementa la cantidad de dinero que almacena en cierta cantidad (amount).
	public void GainMoney(int amount)
	{
		if (!isServer)
			return;

		money += amount;
	}

	// HOOKS...
	void hook_id(int _id) {
		if (DEBUG)
			print ("Id actual: " + id);
	}

	void hook_color(Color _c) {
		if (DEBUG)
			print ("Color actual: " + _c);
	}

	void hook_health(int _health) {
		if (isLocalPlayer) 
		{
			dec_hp1 += old_hp1 -_health;
			old_hp1 = _health;
		} else 
		{
			dec_hp2 += old_hp2 - _health;
			old_hp2 = _health;
		}
		if (DEBUG)
			print ("Vida actual: " + _health);
	}

	void hook_money(int _money) {
		if (isLocalPlayer) 
		{
			GameObject.FindGameObjectWithTag ("money").GetComponent<Text>().text = _money.ToString();
		}
		if (DEBUG)
			print ("Dinero actual: " + _money);
	}

	int dec_hp1 = 0;
	int dec_hp2 = 0;

	int old_hp1 = 100;
	int old_hp2 = 100;

	void Update(){
		if (dec_hp1 > 0) 
		{
			--(GameObject.FindGameObjectWithTag ("hp_j1").GetComponent<Slider> ().value);
			--dec_hp1;
		}
		if (dec_hp2 > 0) 
		{
			--(GameObject.FindGameObjectWithTag ("hp_j2").GetComponent<Slider> ().value);
			--dec_hp2;
		}
	}
}
