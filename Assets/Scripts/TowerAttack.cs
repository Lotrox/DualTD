﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class TowerAttack : NetworkBehaviour {

	private List<GameObject> enemyInRange;
	private float lastHit;
	private bool recentlyPush;
	public AudioClip aClip;
	private AudioSource aSour;

	void Start()
	{
		aSour = GetComponent<AudioSource> ();
		// Contiene a los enemigos.
		enemyInRange = new List<GameObject>();

		// Lee la información de una torre y se lo asigna al tamaño del collider que determina el rango de disparo.
		TowerInfo ti = GetComponent<TowerInfo> ();
		SphereCollider sc = GetComponent<SphereCollider> ();
		sc.radius = (float) ti.range;
		sc.isTrigger = true;

		// Último golpe proporpocionado por la torre
		lastHit = Time.realtimeSinceStartup;

		// Torre recién puesta, lee OnTriggerStay
		recentlyPush = true;
	}

	public void PlaySound(){
		aSour.PlayOneShot (aClip, 0.25f);
	}

	void scanEnemies(Collider other) {
		if (other.gameObject.tag.Equals("Enemigo"))
		{
			SyncOwner enemyOwner = other.gameObject.GetComponent<SyncOwner> (),
			towerOwner = GetComponent<SyncOwner> ();

			if (enemyOwner.getOwner() != towerOwner.getOwner())
			{
				print ("Nuevo enemigo");
				enemyInRange.Add (other.gameObject);
			}

		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (!isServer)
			return;
		
		scanEnemies (other);
	}

	void OnTriggerStay(Collider other) 
	{
		if (!isServer)
		{
			if (recentlyPush)
			{
				recentlyPush = false;
				scanEnemies (other);
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
		if (!ClockTimer.updateable)
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
			if (!IsDestroyed(g)) 
			{
				TowerInfo ti = GetComponent<TowerInfo> ();
				gameObject.transform.GetChild(1).transform.LookAt(g.transform);
				if (Time.realtimeSinceStartup >= (lastHit + ti.speedAttack)) 
				{
					UnitInfo unit = g.GetComponent<UnitInfo> ();
					if (unit.health > 0) {
						lastHit = Time.realtimeSinceStartup;
						RpcSound ();
						//unit.health -= (int)ti.damagePerHit;
						unit.decreaseHealth((int) ti.damagePerHit);
						RpcDamage (g, (float)unit.health/unit.max_health);

					}
					if (unit.health <= 0) {
						if (!IsDestroyed (g)) {
							SyncOwner syncOwner = GetComponent<SyncOwner> ();
							PlayerId playerId = syncOwner.getOwner ().GetComponent<PlayerId> ();
							playerId.GainMoney (unit.money);
							((NetworkMan)NetworkMan.singleton).decreaseUnits();
							Destroy (g);
							print ("Quedan " + (((NetworkMan)NetworkMan.singleton).unitsAlive) + " unidades vivas.");
						} else
							print ("ESTOY MUERTA!");
					}
				}
			}
		}
	}

	public void RpcDamage(GameObject _unit, float _health){
		GameObject A = (((NetworkMan)NetworkMan.singleton).A),
		B = (((NetworkMan)NetworkMan.singleton).B);
		// Daño a los creeps, para actualizar información en el cliente.
		A.GetComponent<NetworkRpc> ().RpcDamage(_unit, _health);
		B.GetComponent<NetworkRpc> ().RpcDamage(_unit,_health);
	
	}

	public void RpcSound(){
		GameObject A = (((NetworkMan)NetworkMan.singleton).A),
			       B = (((NetworkMan)NetworkMan.singleton).B);

		// Rotura de fragmentos.

		A.GetComponent<NetworkRpc> ().RpcSoundTowerAttack (gameObject);
		B.GetComponent<NetworkRpc> ().RpcSoundTowerAttack (gameObject);

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
