using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class TowerSpawner : NetworkBehaviour {

	public static GameObject tower = null;
	GameObject selection;

	enum TowerCase {
		PushConfirm = 0,
		PopConfirm = 1,
		RejectOut = 2,
		RejectMoney = 3
	};
			
	[ClientRpc]
	void RpcTowerBase(TowerCase tc)
	{
		if (!isLocalPlayer)
			return;

		TowerBase (tc);
	}

	void TowerBase(TowerCase tc) {
		switch (tc) 
		{
		case TowerCase.PushConfirm:
			print ("Torre colocada.");
			break;
		case TowerCase.PopConfirm:
			print ("Torre eliminada.");
			break;
		case TowerCase.RejectOut:
			print ("No se puede construír aquí.");
			break;
		case TowerCase.RejectMoney:
			print ("No tienes suficiente dinero.");
			break;
		default:
			break;
		};
	}

	void Start() {
		/*GameObject.Find("/Modelos").transform.Find("CasillasJ1").gameObject.SetActive (false);
		GameObject.Find("/Modelos").transform.Find("CasillasJ2").gameObject.SetActive (false);
		selection = GameObject.Find("/Modelos").transform.Find("CasillasJ" + (GetComponent<PlayerId> ().getId () + 1)).gameObject;*/
	}

	[ClientCallback]
	void Update()
	{
		if (tower == null) 
		{
			//selection.SetActive (false);
			return;
		} 
		else
		{
			//selection.SetActive (true);
		}
		
		if (!ClockTimer.updateable)
			return;
		
		if (!isLocalPlayer)
			return;

		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			Camera.main.GetComponent<freeCam> ().set_aimCursor (false);
		}
			
		
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			
			if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ()) 
			{
				tower = null;
				return;
			}

			Ray vRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit = new RaycastHit ();
			Physics.Raycast (vRay, out hit, 1000);
			GameObject collider = hit.collider.gameObject;

			PlayerId playerId = gameObject.GetComponent<PlayerId> ();

			if (((playerId.getId () == 0) && (!collider.tag.Equals ("casilla_j1"))) || ((playerId.getId () == 1) && (!collider.tag.Equals ("casilla_j2"))))
			{
				TowerBase (TowerCase.RejectOut);
				return;
			}
			print ("CLIENT TOWER:" + tower.name);
			CmdupdateTower (tower);
			CmdSpawn (hit.point, playerId.gameObject, collider); // Lo llaman los clientes!
		}
	}

	[Command]
	void CmdupdateTower(GameObject towerSpawn){
		print ("UPDATE TOWER:" + tower.name);
		tower = towerSpawn;
	}

	// Lo ejecuta el servidor!
	[Command]
	void CmdSpawn(Vector3 point, GameObject player, GameObject collider)
	{
		PlayerId playerId = player.GetComponent<PlayerId> ();
		print ("SPAWN TOWER:" +tower.name);
		TowerInfo towerInfo = tower.GetComponent<TowerInfo> ();
		TowerCase msg;

		if (playerId.money >= towerInfo.cost) {
			GameObject instance = (GameObject)Instantiate (tower, collider.transform.position, collider.transform.rotation);
			instance.GetComponent<SyncTowerBase> ().setTowerBase (collider).deactivate ();
			instance.GetComponent<SyncOwner> ().setOwner (player);
			//instance.GetComponent<SyncColor> ().myColor = c;
			playerId.GainMoney (-towerInfo.cost);
			//playerId.TakeDamage (10);
			//playerId.TakeDamage ((int)towerInfo.damagePerHit);

			NetworkServer.Spawn(instance);

			msg = TowerCase.PushConfirm;
		} 
		else 
		{
			msg = TowerCase.RejectMoney;
		}

		RpcTowerBase (msg);
	}

}
