using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class userUI : MonoBehaviour {

	GameObject menu;

	// Use this for initialization
	void Start () {
		menu = GameObject.Find ("Menu");
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void swapMenu() {
		menu.SetActive (!menu.activeSelf);
	}
		
}
