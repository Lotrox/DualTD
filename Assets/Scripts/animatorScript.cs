using UnityEngine;
using System.Collections;

public class animatorScript : MonoBehaviour {

	public RuntimeAnimatorController ani;

	// Use this for initialization
	void Start () {
		gameObject.GetComponentInChildren<Animator> ().runtimeAnimatorController = ani;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
