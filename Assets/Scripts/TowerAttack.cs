using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class TowerAttack : NetworkBehaviour {

	private List<GameObject> enemyInRange;
	private float lastHit;

	void Start()
	{
		// Contiene a los enemigos.
		enemyInRange = new List<GameObject>();

		// Lee la información de una torre y se lo asigna al tamaño del collider que determina el rango de disparo.
		TowerInfo ti = GetComponent<TowerInfo> ();
		SphereCollider sc = GetComponent<SphereCollider> ();
		sc.radius = (float) ti.range;
		sc.isTrigger = true;

		// Último golpe proporpocionado por la torre
		lastHit = Time.realtimeSinceStartup;
	}

	void OnTriggerEnter(Collider other)
	{
		if (!isServer)
			return;
		
		if (other.gameObject.tag.Equals("Enemigo"))
		{
			print ("Nuevo enemigo");
			SyncOwner enemyOwner = other.gameObject.GetComponent<SyncOwner> (),
					  towerOwner = GetComponent<SyncOwner> ();

			if (enemyOwner != towerOwner)
			{
				print ("Adversario");
				UnitInfo unit = other.gameObject.GetComponent<UnitInfo> ();
				enemyInRange.Add (other.gameObject);
			}

		}
	}

	void OnTriggerExit(Collider other)
	{
		if (!isServer)
			return;
		
		if (enemyInRange.Contains(other.gameObject))
		{
			enemyInRange.Remove(other.gameObject);
		}
	}

	void Update() 
	{
		if (!isServer)
			return;

		// Los enemigos abatidos son eliminados de la lista.
		List<GameObject> toDelete = new List<GameObject> ();
		foreach (GameObject g in enemyInRange) 
		{
			if (IsDestroyed (g))
				toDelete.Add (g);
		}
		enemyInRange.RemoveAll (x => toDelete.Contains(x));

		// Ataca al primer enemigo que se encuentre en la lista.
		if (enemyInRange.Count > 0)
		{
			GameObject g = enemyInRange [0];
			if (g != null) 
			{
				TowerInfo ti = GetComponent<TowerInfo> ();
				if (Time.realtimeSinceStartup > (lastHit + ti.speedAttack)) 
				{
					UnitInfo unit = g.GetComponent<UnitInfo> ();
					if (unit.health > 0) {
						lastHit = Time.realtimeSinceStartup;
						unit.health -= (int) ti.damagePerHit;
					}
					if (unit.health <= 0) {
						--(((NetworkMan)NetworkMan.singleton).unitsAlive);
						print ("Quedan " + (((NetworkMan)NetworkMan.singleton).unitsAlive) + " unidades vivas.");
						Destroy (g);
					}
				}
			}
		}
	}
		

	public static bool IsDestroyed(GameObject go)
	{
		// UnityEngine overloads the == opeator for the GameObject type
		// and returns null when the object has been destroyed, but 
		// actually the object is still there but has not been cleaned up yet
		// if we test both we can determine if the object has been destroyed.
		return go == null && !ReferenceEquals(go, null);
	}
}
