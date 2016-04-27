using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkRpc : NetworkBehaviour {

	// PATRÓN SINGLETON
	protected static NetworkRpc instance;

	public static NetworkRpc Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new NetworkRpc ();
			}

			return instance;
		}
	}

	[ClientRpc]
	public void RpcStandby() {
		print ("Ahora debo esperar 30 segundos.");
	}


}
