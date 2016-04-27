using UnityEngine;
using System.Collections;

public class AgentScript : MonoBehaviour {

	public Transform target;
	NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		//print (target.position.x + target.position.z);
		agent.SetDestination (target.position);
	}
}
