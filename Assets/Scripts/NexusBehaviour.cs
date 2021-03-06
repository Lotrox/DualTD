﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NexusBehaviour : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (!isServer)
			return;
		if (other.tag == "Enemigo") 
		{
			GameObject A = (((NetworkMan)NetworkMan.singleton).A),
					   B = (((NetworkMan)NetworkMan.singleton).B);
			PlayerId playerId = null;

			UnitInfo unit = other.gameObject.GetComponent<UnitInfo> ();
			SyncOwner syncOwner = other.gameObject.GetComponent<SyncOwner> ();

			if (syncOwner.getOwner () != A) 
			{
				playerId = A.GetComponent<PlayerId> ();
			} 
			else 
			{
				playerId = B.GetComponent<PlayerId> ();
			} 
			//if (unit.isBoss)
			playerId.TakeDamage ((int)((float) unit.health * unit.damage / (float)unit.max_health));
			//else
			//	playerId.TakeDamage ((int)unit.damage);
			
			syncOwner.getOwner ().GetComponent<PlayerId> ().GainMoney (unit.money/3);
			Destroy(other.gameObject);
			((NetworkMan)NetworkMan.singleton).decreaseUnits();

			// Rotura de fragmentos.
			A.GetComponent<NetworkRpc> ().RpcNexusUnspawnCrystal (playerId.getId(), playerId.health);
			B.GetComponent<NetworkRpc> ().RpcNexusUnspawnCrystal (playerId.getId(), playerId.health);

			if (playerId.health <= 0) {
				ClockTimer.updateable = false;
			}
		}
	}
}
