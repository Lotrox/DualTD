using UnityEngine;
using System.Collections;

public class position : MonoBehaviour {

	public int x;
	public int z;

	// Use this for initialization
	void Start () {
		x = (int)(transform.parent.position.x / 2.5f);
		z = (int)(transform.parent.position.z / 2.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
