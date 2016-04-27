using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkRpc : NetworkBehaviour {

	static NetworkRpc singleton;

	void Start() {
		singleton = this;
	}

	public static NetworkRpc getInstance(){
		return singleton;
	}

	[ClientRpc]
	public void RpcStandby() {
		print ("Ahora debo esperar 30 segundos.");
	}

}
