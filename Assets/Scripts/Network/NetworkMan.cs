using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkMan : NetworkManager {

	int count;

	GameObject A, B; // Jugadores 1 y 2.

	// https://github.com/fholm/unityassets/blob/master/VoiceChat/Assets/VoiceChat/Scripts/Demo/HLAPI/VoiceChatNetworkManager.cs
	// http://docs.unity3d.com/Manual/UNetPlayers.html (OnServerAddPlayer)

	// Cuando un cliente se desconecta, aparece en el lado servidor.
	public override void OnServerDisconnect(NetworkConnection conn)
	{
		base.OnServerDisconnect (conn);
		--count;
		//NetworkRPC.getInstance().RpcPlayerDisconnect (); // Informar de una desconexión.
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		if (count < 2) {
			player.GetComponent<PlayerId> ().setAttributes(count, new Color(Random.Range(0.0f, 1.0f),Random.Range(0.0f, 1.0f),Random.Range(0.0f, 1.0f),1.0f));
			NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);
		} else {
			if (count == 0)
				A = player;
			else
				B = player;
		}
		++count;
	}
}

