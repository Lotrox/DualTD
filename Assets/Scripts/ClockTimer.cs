using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ClockTimer : MonoBehaviour {

	public AudioClip aClip;
	private AudioSource aSour;
	static float waveTime = -20; // Trata sobre el tiempo desde que se inició la última oleada y/o en curso.
	Text t;
	static float timeWait = 20;
	static bool music = false;
	static bool turningOff = false;

	// Use this for initialization
	void Start () {
		aSour = GetComponent<AudioSource> ();
		t = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {

		if (((Time.realtimeSinceStartup - waveTime) < timeWait) && (Time.realtimeSinceStartup - waveTime) > 0.0f) {
			t.text = (timeWait - (Time.realtimeSinceStartup - waveTime)).ToString ("F2") + " seg";
			GameObject.FindGameObjectWithTag ("wave").GetComponent<Text> ().text = t.text;
			if (!music) {
				print ("Música detenida");
				turningOff = true;
				music = true;
			}

		} else {
			if (music) {
				print ("Reproduciendo música de oleada");
				aSour.volume = 0.5f;
				aSour.PlayOneShot (aClip, 0.5f);
				music = false;
			}
		}
		turnOffMusic ();
	}
		
	public void turnOffMusic(){
		if (turningOff) {
			if (aSour.volume == 0.0f) {
				aSour.Stop ();
				turningOff = false;
			}
			aSour.volume -= 0.05f;
		}
	}

	static public void updateTime(){
		waveTime = Time.realtimeSinceStartup;
	}


}
