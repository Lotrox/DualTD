using UnityEngine;
using System.Collections;

public class UnitInfo : MonoBehaviour {

	private Object semaphore = new Object();

	public float damage = 5; // Daño que provoca en el nexo.
	public float max_health = 10; // Vida de la unidad.
	public float health = 10; // Vida de la unidad.
	public int money = 2; // Dinero obtenido al matar a la unidad.
	public double resistance = 5; // Resistencia al daño.
	public float speed = 1; // Velocidad de movimiento.
	public bool isBoss = false;
	public GameObject health_bar;
	void Start() {
		max_health = health;
		NavMeshAgent nma = GetComponent<NavMeshAgent> ();
		nma.speed = speed;
		nma.acceleration = 5;
	}
	public void UpdateUnit(){
		max_health = health;
		NavMeshAgent nma = GetComponent<NavMeshAgent> ();
		nma.speed = speed;
	}

	public void decreaseHealth(int amount) {
		lock (semaphore) {
			health -= amount;
		}
	}
}
