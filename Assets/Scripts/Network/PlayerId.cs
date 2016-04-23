using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerId : NetworkBehaviour {

	[SyncVar]
	int id;

	[SyncVar]
	Color c;

	public void Start(){
		// Asignación de cámaras.
		if (id == 0) {
			Camera.main.transform.position = new Vector3 (18.0f, 30.0f, -24.0f);
			Camera.main.GetComponent<freeCam>().updateCamera (false);
		} else if (id == 1) {
			Camera.main.transform.position = new Vector3 (82.0f, 30.0f, 124.0f);
			Camera.main.GetComponent<freeCam>().updateCamera (true);
		} 
	}

	public void setAttributes(int _id, Color _c) {
		id = _id;
		c = _c;
	}

	public int getId() {
		return id;
	}

	public Color getColor() {
		return c;
	}
}
