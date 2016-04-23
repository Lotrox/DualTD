using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TowerSpawner : NetworkBehaviour {

	public GameObject tower;

	[ClientCallback]
	void Update(){
		if (!isLocalPlayer)
			return;
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			Camera coffee = Camera.main;
			Ray vRay = coffee.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit = new RaycastHit ();
			Physics.Raycast (vRay, out hit, 1000);
			GameObject goc = hit.collider.gameObject;
			if ((!goc.tag.Equals("casilla_j1")) && (!goc.tag.Equals("casilla_j2")))
				return;
			PlayerId id = gameObject.GetComponent<PlayerId> ();
			if ((id.getId() == 0) && (!goc.tag.Equals ("casilla_j1")))
				return;
			if ((id.getId() == 1) && (!goc.tag.Equals ("casilla_j2")))
				return;
			CmdSpawn (hit.point, id.getColor(), gameObject, goc); // Lo llaman los clientes!
			// goc.SetActive(false);
		}
	}

	// Lo ejecuta el servidor!
	[Command]
	void CmdSpawn(Vector3 point, Color c, GameObject gop, GameObject goc) {
		Money m = gop.GetComponent<Money> ();
		Health h = gop.GetComponent<Health> ();
		TowerInfo ti = tower.GetComponent<TowerInfo> ();
		if (m.currentMoney >= ti.cost) {
			GameObject instance = (GameObject)Instantiate (tower, goc.transform.position, goc.transform.rotation);
			instance.GetComponent<SyncTowerBase> ().setTowerBase (goc).deactivate ();
			instance.GetComponent<SyncOwner> ().setOwner (gop);
			//instance.GetComponent<SyncColor> ().myColor = c;
			m.GainMoney (-ti.cost);
			h.TakeDamage ((int)ti.damagePerHit);
			NetworkServer.SpawnWithClientAuthority (instance, base.connectionToClient);
		}
	}
		
}
