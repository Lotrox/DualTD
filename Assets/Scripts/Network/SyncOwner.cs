using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SyncOwner : NetworkBehaviour {

	[SyncVar]
	GameObject owner;

	public void setOwner(GameObject go) {
		owner = go;
	}

	public GameObject getOwner() {
		return owner;
	}
}
