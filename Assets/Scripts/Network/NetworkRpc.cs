using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class NetworkRpc : NetworkBehaviour {

	public GameObject creep;
	private Transform[] s = new Transform[2]; // Start o Salida del Jugador 1/2
	private Transform[] e = new Transform[2]; // End o Entrada Jugador 2

	void Start() 
	{
		s[0] = GameObject.FindGameObjectWithTag ("S1").transform;
		e[0] = GameObject.FindGameObjectWithTag ("E1").transform;
		s[1] = GameObject.FindGameObjectWithTag ("S2").transform;
		e[1] = GameObject.FindGameObjectWithTag ("E2").transform;
	}

	[ClientRpc]
	public void RpcStandby() {
		if (!isLocalPlayer)
			return;
		ClockTimer.updateTime ();
		print ("Ahora debo esperar 20 segundos.");
	}

	[ClientRpc]
	public void RpcSpawnUnits(int wave) 
	{
		if (!isLocalPlayer)
			return;
		GameObject.FindGameObjectWithTag ("wave").GetComponent<Text> ().text = "Oleada " + wave;
		CmdSpawnUnits (gameObject, wave);
	}

	[Command]
	public void CmdSpawnUnits(GameObject player, int wave) 
	{
		for (int i = 0; i <= wave; ++i) 
		{
			PlayerId playerId = player.GetComponent<PlayerId> ();
			print ("Spawn de unidad perteneciente al jugador " + playerId.getId ());
			creep.GetComponent<AgentScript> ().target = e[playerId.getId()];
			GameObject instance = (GameObject)Instantiate (creep, s [playerId.getId ()].position, s [playerId.getId ()].rotation);

			instance.GetComponent<SyncOwner> ().setOwner (player);

			NetworkServer.Spawn (instance);
			++(((NetworkMan)NetworkMan.singleton).unitsAlive);
		}
	}

	[ClientRpc]
	public void RpcNexusUnspawnCrystal(int id, int health) 
	{
		if (!isLocalPlayer)
			return;

		GameObject nexus = GameObject.Find ("/Modelos/Nexo_J" + (id + 1));

		for (int i = 90; i >= 0; i -= 10) 
		{
			if (health <= i) 
			{
				nexus.transform.FindChild("Crystal_" + i).gameObject.SetActive (false);
			}
		}
		if (health <= 0) 
		{
			nexus.transform.FindChild ("Luces").gameObject.SetActive (false);
		}
	}

	[ClientRpc]
	public void RpcWinByDisconnection() {
		print ("Tu rival se ha desconectado, por lo tanto, se te autodeclara victoria.");
		//Network.Disconnect();
		//MasterServer.UnregisterHost();
	}
}
