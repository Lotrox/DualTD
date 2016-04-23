using UnityEngine;
using System.Collections;

public class Flicking : MonoBehaviour {

	public Material m1;
	public Material m2;
	private Material m;
	public float maxA = 0.9f; // Máxima amplitud de la onda.
	public float minA = 0.8f; // Mínima amplitud de la onda.
	public float f = 0.5f; // Frecuencia de la onda.

	public void swapMaterial(bool playerOne){
		if (playerOne)
			m = m1;
		else
			m = m2;
	}

	// Update is called once per frame
	void Update () {
		if (m == null)
			return;

		float A = (maxA - minA);  

		float s = 2.0f * Mathf.PI * f * Time.timeSinceLevelLoad;
		float y = Mathf.Sin(s);
		y = y * A / 2 - (minA - 1) / 2;

		m.SetColor("_TintColor", new Color(1.0f, 1.0f, 1.0f, y));
	}
}
