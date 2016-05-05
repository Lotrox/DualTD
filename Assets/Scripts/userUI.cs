using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class userUI : MonoBehaviour {

	GameObject menu;
	static private Text t; 
	static float timer = 0;
	static bool disable = false;

	// Use this for initialization
	void Start () {
		menu = GameObject.Find ("Menu");
		t = GameObject.FindGameObjectWithTag ("error").gameObject.GetComponent<Text>();
		t.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.realtimeSinceStartup - timer > 3) {
			t.text = "";
		}
	}

	static public void notifyError(string name){
		t.text = name;
		timer = Time.realtimeSinceStartup;
	}

	public void swapMenu() {
		menu.SetActive (!menu.activeSelf);
	}
		
}
