using UnityEngine;
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
		
		print ("Enter enter");

		GameObject A = (((NetworkMan)NetworkMan.singleton).A),
				   B = (((NetworkMan)NetworkMan.singleton).B);
		PlayerId playerId = null;

		/*UnitInfo unit = other.GetComponent<UnitInfo> ();
		SyncOwner syncOwner = other.GetComponent<SyncOwner> ();

		if (syncOwner.getOwner () != A) 
		{
			playerId = A.GetComponent<PlayerId> ();
		} 
		else 
		{
			playerId = B.GetComponent<PlayerId> ();
		} */

		//playerId.TakeDamage (unit.damage);
		NetworkServer.UnSpawn (other.gameObject);
	}
}
