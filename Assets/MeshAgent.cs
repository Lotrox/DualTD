using UnityEngine;
using System.Collections;

public class MeshAgent : MonoBehaviour {


	public Transform goal;

	void Start () {
		NavMeshAgent agent = GetComponent<NavMeshAgent>();
		agent.SetDestination (goal.position);
	}
}
