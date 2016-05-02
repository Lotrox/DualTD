using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkMan : NetworkManager {

	int count;
	int wave = 0;
	int unitsPerWave = 3;
	bool waveSpawned = false;
	float globalTime, // Trata sobre el tiempo global desde que se inició la partida.
	      waveTime; // Trata sobre el tiempo desde que se inició la última oleada y/o en curso.

	GameObject A, B; // Jugadores 1 y 2.
	public int unitsAlive = 0; // Unidades vivas en una oleada.

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
		if (count < 2) 
		{
			player.GetComponent<PlayerId> ().setAttributes (count, new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), 1.0f));
			NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);

			if (count == 0) 
			{
				A = player;
			} 
			else 
			{
				B = player;
				waveTime = globalTime = Time.realtimeSinceStartup;
				print ("Ha comenzado la partida");
				NetworkRpc.getInstance ().RpcStandby ();
			}
		}
		++count;
	}

	void Update()
	{
		if (count >= 2) // Los 2 jugadores están en partida.
		{
			if (Time.realtimeSinceStartup >= waveTime + 30.0f)
			{
				if (!waveSpawned) 
				{
					waveTime = Time.realtimeSinceStartup;
					spawnUnits ();
					waveSpawned = true;
				}
				if (waveFinished ()) 
				{
					waveTime = Time.realtimeSinceStartup;
					waveSpawned = false;
					// Descanso de 30 segundos.
					NetworkRpc.getInstance().RpcStandby();
				}
			}
		}
		else 
		{
			// Falta algún jugador.
		}
	}
		
	bool waveFinished() {
		if (unitsAlive == 0)
			print ("La oleada ha finalizado");
		
		return (unitsAlive == 0);
	}
		
	void spawnUnits() {
		++wave;

		// El servidor notifica que los jugadores van a crear unidades. (Necesita su autoridad).
		NetworkRpc.getInstance ().RpcSpawnUnits (wave);
	}
		
}

