using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ClockTimer : MonoBehaviour {

	float waveTime; // Trata sobre el tiempo desde que se inició la última oleada y/o en curso.
	Text t;
	float timeWait = 20.0f;

	// Use this for initialization
	void Start () {
		t = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		//if (Time.realtimeSinceStartup >= waveTime + 30.0f) {
		if (GameObject.FindGameObjectWithTag ("wave").GetComponent<Text> ().text == "20 seg")
			waveTime = Time.realtimeSinceStartup;
		else
			return;
		if ((timeWait - Time.realtimeSinceStartup - waveTime) > 1){
			t.text = (timeWait - Time.realtimeSinceStartup - waveTime).ToString("F2") + " seg";
			GameObject.FindGameObjectWithTag ("wave").GetComponent<Text>().text = t.text;
		}
	}


}
