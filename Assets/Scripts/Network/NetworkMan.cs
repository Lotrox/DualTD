﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetworkMan : NetworkManager {

	int count;
	int wave = 0;
	bool waveSpawned = false;
	float waveTime; // Trata sobre el tiempo desde que se inició la última oleada y/o en curso.

	Object semaphore = new Object();

	public GameObject A, B; // Jugadores 1 y 2.
	private NetworkConnection Ac, Bc;

	public int unitsAlive = 0; // Unidades vivas en una oleada.
	bool firstWave = true;
	bool init = false;

	// https://github.com/fholm/unityassets/blob/master/VoiceChat/Assets/VoiceChat/Scripts/Demo/HLAPI/VoiceChatNetworkManager.cs
	// http://docs.unity3d.com/Manual/UNetPlayers.html (OnServerAddPlayer)

	// Cuando un cliente se desconecta, aparece en el lado servidor.
	public override void OnServerDisconnect(NetworkConnection conn)
	{
		base.OnServerDisconnect (conn);
		count = int.MinValue;
		if (Ac == conn)
			B.GetComponent<NetworkRpc> ().RpcWinByDisconnection ();
		if (Bc == conn)
			A.GetComponent<NetworkRpc> ().RpcWinByDisconnection ();
		//SceneManager.LoadScene ("main");
		//NetworkRPC.getInstance().RpcPlayerDisconnect (); // Informar de una desconexión.
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		GameObject player = null;
		if (!init)
		{
			player = (GameObject)Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);
			if (count < 2)
			{
				player.GetComponent<PlayerId> ().setAttributes (count, new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), 1.0f));
				NetworkServer.AddPlayerForConnection (conn, player, playerControllerId);

				if (count == 0)
				{
					A = player;
					Ac = conn;
				} else
				{
					B = player;
					Bc = conn;
					init = true;
					waveTime = Time.realtimeSinceStartup;
					print ("Ha comenzado la partida");
					A.GetComponent<NetworkRpc> ().RpcStandby ();
					B.GetComponent<NetworkRpc> ().RpcStandby ();
				}
			}
		}
		++count;
	}

	void Update()
	{
		if (!ClockTimer.updateable)
			return;
		
		if (count >= 2)
		{ // Los 2 jugadores están en partida.
			if (firstWave)
			{
				firstWave = !firstWave;
				A.GetComponent<PlayerId> ().GainMoney (25);
				B.GetComponent<PlayerId> ().GainMoney (25);
			}
			
			if (!waveSpawned) 
			{
				if (Time.realtimeSinceStartup >= waveTime + 20.0f) 
				{
					spawnUnits ();
					waveSpawned = true;
				}
			} 
			else if (waveFinished ()) 
			{
				waveTime = Time.realtimeSinceStartup;
				waveSpawned = false;
				// Descanso de 20 segundos.
				A.GetComponent<NetworkRpc> ().RpcStandby ();
				B.GetComponent<NetworkRpc> ().RpcStandby ();
			}
		} else 
		{
			// Falta algún jugador.
		}
	}
		
	bool waveFinished() {
		return (GameObject.FindGameObjectsWithTag ("Enemigo").Length <= 0);
	}
		
	void spawnUnits() {
		++wave;

		// El servidor notifica que los jugadores van a crear unidades. (Necesita su autoridad).
		A.GetComponent<NetworkRpc> ().RpcSpawnUnits (wave);
		B.GetComponent<NetworkRpc> ().RpcSpawnUnits (wave);
	}


	// Incrementa el número de unidades.
	public void increaseUnits()
	{
		lock (semaphore) {
			++unitsAlive;
		}
	}

	// Decrementa el número de unidades.
	public void decreaseUnits() 
	{
		lock (semaphore) {
			--unitsAlive;
		}
	}
}

