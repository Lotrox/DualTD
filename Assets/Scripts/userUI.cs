using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class userUI : MonoBehaviour {

	GameObject menu;
	static private Text t; 
	static float timer = 0;
	// static bool disable = false;
	public AudioClip buildTower;
	public AudioClip destroy;
	public AudioClip error;
	static private AudioSource aSour;
	static public int audioSelect = -1;

	// Use this for initialization
	void Start () {
		aSour = GetComponent<AudioSource> ();
		menu = GameObject.Find ("Menu");
		t = GameObject.FindGameObjectWithTag ("error").gameObject.GetComponent<Text>();
		t.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.realtimeSinceStartup - timer > 3) {
			t.text = "";
		}
		if (audioSelect != -1) {
			if (audioSelect == 0)
				aSour.PlayOneShot (buildTower, 0.7f);
			if (audioSelect == 1)
				aSour.PlayOneShot (error, 0.2f);
			if (audioSelect == 2)
				aSour.PlayOneShot (destroy, 0.5f);
			audioSelect = -1;
		}
	}

	static public void notifyError(string name){
		t.text = name;
		timer = Time.realtimeSinceStartup;
	}

	// 0: Construir torre. 1: Error. 2: Eliminar torre.
	static public void notifyAction(int c){
		audioSelect = c;
	}
		

	public void swapMenu() {
		menu.SetActive (!menu.activeSelf);
	}
		
}
