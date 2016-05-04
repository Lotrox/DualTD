using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AgentScript : NetworkBehaviour {

	[SyncVar]
	public Transform target;

	NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		//print (target.position.x + target.position.z);
		agent.SetDestination (target.position);
	}
	
	// Update is called once per frame
	void Update () {
		if (!ClockTimer.updateable)
			agent.SetDestination(transform.position);
	}
}
