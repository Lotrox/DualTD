using UnityEngine;
using System.Collections;

public class AutoSpawn : MonoBehaviour {

	public GameObject prefab;
	ConcreteGrid cg;

	// Use this for initialization
	void spawner () {
		cg = GetComponent<ConcreteGrid> ();
		if (cg != null) {
			uint cols = cg.getCols (),
				 rows = cg.getRows ();
			double width = cg.getWidth (),
				   height = cg.getHeight ();

			for (uint col = 0; col < cols; ++col) {
				for (uint row = 0; row < rows; ++row) {
					print (col + ", " + row);
					GameObject.Instantiate (prefab, new Vector3 ((float) (col * width), -2.0f, (float) (row * height)), Quaternion.identity);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			print ("HOLA");
			spawner ();
		}
	}
}
