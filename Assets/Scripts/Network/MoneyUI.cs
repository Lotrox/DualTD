using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MoneyUI : Money {

	private Text Am, Bm;

	void Start(){
		GameObject panel = GameObject.FindWithTag ("UI");
		Am = panel.GetComponentsInChildren<Text> () [2];
		Bm = panel.GetComponentsInChildren<Text> () [3];
		CurrentMoney (currentMoney);
	}

	void OnPlayerConnected(){ // Llamado por el servidor cuando detecta un nuevo jugador conectado.
		CurrentMoney (currentMoney);
	}

	override protected void CurrentMoney(int _currentMoney) {
		print ("Dinero sincronizado: " + _currentMoney);
		if (isLocalPlayer)
			Am.text = "Dinero: " + _currentMoney;
		else
			Bm.text = "Dinero: " + _currentMoney;
	}
}
