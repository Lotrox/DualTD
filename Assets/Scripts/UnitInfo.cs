using UnityEngine;
using System.Collections;

public class UnitInfo : MonoBehaviour {

	public int damage = 5; // Daño que provoca en el nexo.
	public int max_health = 10; // Vida de la unidad.
	public int health = 10; // Vida de la unidad.
	public int money = 2; // Dinero obtenido al matar a la unidad.
	public double resistance = 5; // Resistencia al daño.
	public float speed = 1; // Velocidad de movimiento.

	public GameObject health_bar;
	void Start() {
		max_health = health;
		NavMeshAgent nma = GetComponent<NavMeshAgent> ();
		nma.speed = speed;
		nma.acceleration = 5;
	}
	void Update(){
//		float norm_health = (float)health / max_health;
//		if (norm_health <= 0)
//			norm_health = 0;
//		health_bar.transform.localScale = new Vector3((float)health / max_health, health_bar.transform.localScale.y, health_bar.transform.localScale.z);
	}
}
