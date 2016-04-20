using UnityEngine;
using System.Collections;

public class AutoSpawn : MonoBehaviour {

	public GameObject prefab;
	ConcreteGrid cg;

	// Use this for initialization
	void Start () {
		cg = GetComponent<ConcreteGrid> ();
		if (cg != null) {
			uint cols = cg.getCols (),
				 rows = cg.getRows ();
			double width = cg.getWidth (),
				   height = cg.getHeight ();

			for (uint col = 0; col < cols; ++col) {
				for (uint row = 0; row < rows; ++row) {
					GameObject.Instantiate (prefab, new Vector3 ((float) (col * width), 0.0f, (float) (row * height)), Quaternion.identity);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
