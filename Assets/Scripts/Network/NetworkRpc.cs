﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class NetworkRpc : NetworkBehaviour {

	public GameObject creep;
	public GameObject boss;
	private Transform[] s = new Transform[2]; // Start o Salida del Jugador 1/2
	private Transform[] e = new Transform[2]; // End o Entrada Jugador 2


	void Start() 
	{
		s[0] = GameObject.FindGameObjectWithTag ("S1").transform;
		e[0] = GameObject.FindGameObjectWithTag ("E1").transform;
		s[1] = GameObject.FindGameObjectWithTag ("S2").transform;
		e[1] = GameObject.FindGameObjectWithTag ("E2").transform;
	}

	[ClientRpc]
	public void RpcStandby() {
		if (!isLocalPlayer)
			return;
		ClockTimer.updateTime ();
		print ("Ahora debo esperar 20 segundos.");
	}

	[ClientRpc]
	public void RpcSpawnUnits(int wave) 
	{
		if (!isLocalPlayer)
			return;
		GameObject.FindGameObjectWithTag ("wave").GetComponent<Text> ().text = "Oleada " + wave;
		CmdSpawnUnits (gameObject, wave);
	}

	[Command]
	public void CmdSpawnUnits(GameObject player, int wave) 
	{
		int num = wave;
		float damage, health;
		if ((wave % 5) == 0) // Oleada del jefe
		{
			num = wave / 5;
			health = (num - 1) * 100; // 100 -> 500 * 2 -> 1000 * 3 -> 15000 * 4...
			damage = (num - 1) * 50; 
		}
		else 
		{
			health = (wave + wave/5) * Mathf.Log (wave + wave/5);
			damage = (wave + wave/5) * Mathf.Log (wave + wave/5);

			num++; // Incrementar número de unidades.
			if (num >= 10) 
				num = 10;
		}
			
		
		for (int i = 0; i < num; ++i) 
		{
			PlayerId playerId = player.GetComponent<PlayerId> ();
			GameObject instance;
			print ("Spawn de unidad perteneciente al jugador " + playerId.getId ());
			if ((wave % 5) == 0) 
			{
				boss.GetComponent<AgentScript> ().target = e [playerId.getId ()];
				instance = (GameObject)Instantiate (boss, s [playerId.getId ()].position, s [playerId.getId ()].rotation);
			} 
			else 
			{
				creep.GetComponent<AgentScript> ().target = e [playerId.getId ()];
				instance = (GameObject)Instantiate (creep, s [playerId.getId ()].position, s [playerId.getId ()].rotation);
			}

			instance.GetComponent<SyncOwner> ().setOwner (player);
			instance.GetComponent<SyncUnitInfo> ().setParams (damage, health);
			NetworkServer.Spawn (instance);
			(((NetworkMan)NetworkMan.singleton)).increaseUnits();
		}
	}

	public void endCanvas(string t, string s) {
		GameObject result = GameObject.Find ("/Canvas").transform.Find("Resultado").gameObject;
		Text title = result.transform.GetChild (0).gameObject.GetComponent<Text>();
		Text subtitle = (Text)result.transform.GetChild (1).gameObject.GetComponent <Text> ();
		Button exit = (Button) result.transform.GetChild (2).gameObject.GetComponent<Button>();

		title.text = t;
		subtitle.text = s;
		exit.onClick.AddListener(() => { Application.Quit(); });

		result.SetActive (true);
		ClockTimer.updateable = false;
	}

	[ClientRpc]
	public void RpcNexusUnspawnCrystal(int id, int health) 
	{
		if (!isLocalPlayer)
			return;

		GameObject nexus = GameObject.Find ("/Modelos/Nexo_J" + (id + 1));
		bool myself = (GetComponent<PlayerId> ().getId () == id);


		for (int i = 90; i >= 0; i -= 10) 
		{
			if (health <= i) 
			{
				nexus.transform.FindChild("Crystal_" + i).gameObject.SetActive (false);
			}
		}
		if (health <= 0) 
		{
			nexus.transform.FindChild ("Luces").gameObject.SetActive (false);
			if (myself)
			{
				endCanvas ("Has perdido", "Todos tenemos errores");
			} else 
			{
				endCanvas ("Has ganado", "Enhorabuena");
			}
		}
	}

	[ClientRpc]
	public void RpcWinByDisconnection()
	{
		print ("Tu rival se ha desconectado, por lo tanto, se te autodeclara victoria.");

		endCanvas ("Has ganado", "Tu rival se ha desconectado.");

		//Network.Disconnect();
		//MasterServer.UnregisterHost();
	}

	[ClientRpc]
	public void RpcSoundTowerAttack(GameObject go){
		go.GetComponent<TowerAttack>().PlaySound ();
	
	}

	[ClientRpc]
	public void RpcDamage(GameObject _unit, float norm_health){
		if(_unit != null){
			GameObject hbar = _unit.GetComponent<UnitInfo> ().health_bar;
			_unit.GetComponent<UnitInfo>().health_bar.transform.localScale = new Vector3(norm_health, hbar.transform.localScale.y, hbar.transform.localScale.z);
		}
	}
}
