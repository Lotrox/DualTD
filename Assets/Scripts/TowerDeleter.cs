using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TowerDeleter : NetworkBehaviour {

	public static bool readyForDelete = false;

	[ClientCallback]
	void Update () {
		if (!isLocalPlayer)
			return;

		if (!ClockTimer.updateable)
			return;

		if (Input.GetKeyDown (KeyCode.Mouse0) && readyForDelete) {
			Camera.main.GetComponent<freeCam> ().set_aimCursor (false);

			if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ()) 
			{
				readyForDelete = false;
				return;
			}

			Ray vRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit = new RaycastHit ();
			Physics.Raycast (vRay, out hit, 1000);
			GameObject collider = hit.collider.gameObject.transform.parent.gameObject;

			TowerInfo ti = collider.GetComponent<TowerInfo> ();
			if (ti != null) {
				GameObject owner = collider.GetComponent<SyncOwner> ().getOwner ();
				if (owner == gameObject) {
					CmdDestroyer (collider);
					userUI.notifyAction (2);
				} else {
					userUI.notifyError ("La torre que intentas borrar no te pertenece. Es de tu rival.");
					userUI.notifyAction(1);
				}
			} else {
				userUI.notifyError ("No se puede eliminar esto. No es una torre.");
				userUI.notifyAction(1);
			}

			readyForDelete = false;
		}
	}

	[Command]
	void CmdDestroyer(GameObject tower) {
		tower.GetComponent<SyncTowerBase> ().activate ();
		Destroy (tower);
	}
}
