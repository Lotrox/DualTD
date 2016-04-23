using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SyncTowerBase : NetworkBehaviour {

	[SyncVar]
	GameObject towerBase;

	public SyncTowerBase setTowerBase(GameObject go) {
		towerBase = go;
		return this;
	}

	public void activate() {
		towerBase.SetActive (true);
	}

	public void deactivate() {
		towerBase.SetActive (false);
	}
}
