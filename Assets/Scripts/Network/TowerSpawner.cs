using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class TowerSpawner : NetworkBehaviour {

	public static GameObject tower = null; // Torre seleccionada por el jugador.
	public GameObject[] towers = new GameObject[10]; // Estructura de torres usada por el servidor.
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
			//userUI.notifyError("Torre colocada");
			userUI.notifyAction(0);
			break;
		case TowerCase.PopConfirm:
			//userUI.notifyError ("Torre eliminada");
			userUI.notifyAction(2);
			break;
		case TowerCase.RejectOut:
			userUI.notifyError ("No se puede construír aquí");
			userUI.notifyAction(1);
			break;
		case TowerCase.RejectMoney:
			userUI.notifyError ("No tienes suficiente dinero");
			userUI.notifyAction(1);
			break;
		default:
			break;
		};
	}

	[ClientCallback]
	void Start() {
		if (!isLocalPlayer)
			return;

		if (GetComponent<PlayerId> ().getId () == 0)
			selection = GameObject.Find("/Modelos").transform.Find("CasillasJ1").gameObject;
		else 
			selection = GameObject.Find("/Modelos").transform.Find("CasillasJ2").gameObject;

		GameObject.Find("/Modelos").transform.Find("CasillasJ1").gameObject.SetActive (false);
		GameObject.Find("/Modelos").transform.Find("CasillasJ2").gameObject.SetActive (false);
	}

	[ClientCallback]
	void Update()
	{
		if (!isLocalPlayer)
			return;
		
		if (tower == null) 
		{
			selection.SetActive (false);
			return;
		} 
		else
		{
			selection.SetActive (true);
		}
		
		if (!ClockTimer.updateable)
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
				tower = null;
				return;
			}
			CmdSpawn (hit.point, playerId.gameObject, collider, tower.GetComponent<TowerInfo>().id); // Lo llaman los clientes!
			tower = null;
		}
	}
		
	// Lo ejecuta el servidor!
	[Command]
	void CmdSpawn(Vector3 point, GameObject player, GameObject collider, int id)
	{
		PlayerId playerId = player.GetComponent<PlayerId> ();
		TowerInfo towerInfo = towers[id].GetComponent<TowerInfo> ();
		TowerCase msg;

		if (playerId.money >= towerInfo.cost) {
			GameObject instance = (GameObject)Instantiate (towers[id], collider.transform.position, collider.transform.rotation);
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
