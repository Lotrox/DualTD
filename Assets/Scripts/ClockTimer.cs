using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/*IMPORTANTE: Esta script se encuentra en el gameobject MONEY.*/

public class ClockTimer : MonoBehaviour {

	public static bool updateable = true;

	public AudioClip clipWave, clipWait, clipGong, clipBoss;
	private AudioSource aSour;
	static float waveTime = -20; // Trata sobre el tiempo desde que se inició la última oleada y/o en curso.
	Text t;
	static float timeWait = 20;
	static int numWave = 0;
	static bool music = false;
	static bool turningOff = false;
	static bool turningOn = false;

	// Use this for initialization
	void Start () {
		aSour = GetComponent<AudioSource> ();
		t = GetComponent<Text> ();
		aSour.PlayOneShot (clipWait, 0.5f);
		t.text = "Esperando...";
		aSour.Stop ();
		waveTime = -20;
		music = false;
		turningOff = false;
		GameObject.Find("/Modelos/Nexo_J1").transform.Find("Luces").gameObject.SetActive (false);
		GameObject.Find("/Modelos/Nexo_J2").transform.Find("Luces").gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (!updateable)
			return;
		
		if (((Time.realtimeSinceStartup - waveTime) < timeWait) && (Time.realtimeSinceStartup - waveTime) > 0.0f) {
			t.text = (timeWait - (Time.realtimeSinceStartup - waveTime)).ToString ("F2") + " seg";
			GameObject.FindGameObjectWithTag ("wave").GetComponent<Text> ().text = t.text;
			if (!music) {
				print ("Música detenida.");
				turningOff = true;
				music = true;
			}

		} else {
			if (music) {
				GameObject.Find("/Modelos/Nexo_J1").transform.Find("Luces").gameObject.SetActive (true);
				GameObject.Find("/Modelos/Nexo_J2").transform.Find("Luces").gameObject.SetActive (true);
				print ("Reproduciendo música de oleada.");
				++numWave;
				aSour.Stop ();
				aSour.volume = 0.5f;
				aSour.PlayOneShot (clipGong, 0.5f);
				turningOn = true;
				music = false;
			}
			if(numWave == 0) 
				GameObject.FindGameObjectWithTag ("wave").GetComponent<Text> ().text = "Esperando...";
			else
				GameObject.FindGameObjectWithTag ("wave").GetComponent<Text> ().text = "Oleada " + numWave;
		}
		turnOffMusic ();
		turnOnMusic ();
	}
		
	public void turnOffMusic(){
		if (turningOff) {
			if (aSour.volume == 0.0f) {
				aSour.Stop ();
				turningOff = false;
				aSour.volume = 0.2f;
				aSour.PlayOneShot (clipWait, 0.2f);
			}
			aSour.volume -= 0.05f;
		}
	}

	public void turnOnMusic(){
		if (turningOn) {
			if (aSour.volume < 0.2f) {
				turningOn = false;
				aSour.volume = 0.6f;
				if(numWave%5 == 0)
					aSour.PlayOneShot (clipBoss, 0.5f);
				else
					aSour.PlayOneShot (clipWave, 0.5f);
			}
			aSour.volume -= 0.05f;
		}
	}

	static public void updateTime(){
		waveTime = Time.realtimeSinceStartup;
	}


}
