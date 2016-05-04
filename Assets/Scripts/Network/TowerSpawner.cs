using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class TowerSpawner : NetworkBehaviour {

	public static GameObject tower = null;

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

	[ClientCallback]
	void Update()
	{
		if (tower == null)
			return;
		
		if (!ClockTimer.updateable)
			return;
		
		if (!isLocalPlayer)
			return;

		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			Camera.main.GetComponent<freeCam> ().set_aimCursor (false);
		}

		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ())
			return;
		
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
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

			CmdSpawn (hit.point, playerId.gameObject, collider); // Lo llaman los clientes!
			tower = null;
		}
	}

	// Lo ejecuta el servidor!
	[Command]
	void CmdSpawn(Vector3 point, GameObject player, GameObject collider)
	{
		PlayerId playerId = player.GetComponent<PlayerId> ();
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
