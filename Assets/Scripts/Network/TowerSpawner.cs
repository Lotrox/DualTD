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
		}
	}

	// Lo ejecuta el servidor!
	[Command]
	void CmdSpawn(Vector3 point, Color c, GameObject gop, GameObject goc) {
		//Money m = go.GetComponent<Money> ();
		//if (m.currentMoney >= 10) {
		
		GameObject instance = (GameObject)Instantiate (tower, goc.transform.position, goc.transform.rotation);
			//instance.GetComponent<SyncColor> ().myColor = c;
		//	instance.GetComponent<SyncOwner> ().setOwner (gop);
			//go.GetComponent<Money> ().GainMoney (-10);
			NetworkServer.SpawnWithClientAuthority (instance, base.connectionToClient);
		//}
	}
		
}
