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
		print ("Ahora debo esperar 30 segundos.");
	}

	[ClientRpc]
	public void RpcSpawnUnits(int wave) {
		if (!isLocalPlayer)
			return;
		CmdSpawnUnits (gameObject, wave);
		GameObject.FindGameObjectWithTag ("wave").GetComponent<Text> ().text = "Oleada " + wave;
		print ("Se han generado los súbditos para la oleada " + wave);
		print ("Dicha oleada pertenece al " + gameObject.GetComponent<PlayerId> ().getId ());
	}

	[Command]
	public void CmdSpawnUnits(GameObject player, int wave) 
	{
		for (int i = 0; i <= wave; ++i) 
		{
			PlayerId playerId = player.GetComponent<PlayerId> ();
			print ("Spanw de unidad perteneciente al jugador " + playerId.getId ());
			creep.GetComponent<AgentScript> ().target = e[playerId.getId()];
			GameObject instance = (GameObject)Instantiate (creep, s [playerId.getId ()].position, s [playerId.getId ()].rotation);

			instance.GetComponent<SyncOwner> ().setOwner (player);

			NetworkServer.SpawnWithClientAuthority (instance, base.connectionToClient);
			++(((NetworkMan)NetworkMan.singleton).unitsAlive);
		}
	}
}
