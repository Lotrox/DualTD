using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ClockTimer : MonoBehaviour {

	static float waveTime = 20; // Trata sobre el tiempo desde que se inició la última oleada y/o en curso.
	Text t;
	static float timeWait = 20;

	// Use this for initialization
	void Start () {
		t = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (((Time.realtimeSinceStartup - waveTime) < timeWait - 0.5f) && (Time.realtimeSinceStartup - waveTime) > 0.0f){
			t.text = (timeWait - Time.realtimeSinceStartup - waveTime).ToString("F2") + " seg";
			GameObject.FindGameObjectWithTag ("wave").GetComponent<Text>().text = t.text;
		}
	}

	static public void updateTime(){
		waveTime = Time.realtimeSinceStartup;
	}


}
