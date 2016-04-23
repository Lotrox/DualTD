using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthUI : Health {

	private Text A, B;

	void Start(){
		GameObject panel = GameObject.FindWithTag ("UI");
		A = panel.GetComponentsInChildren<Text> () [0];
		B = panel.GetComponentsInChildren<Text> () [1];
		CurrentHealth (currentHealth);
	}

	void OnPlayerConnected(){ // Llamado por el servidor cuando detecta un nuevo jugador conectado.
		CurrentHealth (currentHealth);
	}
		
	override protected void CurrentHealth(int _currentHealth) {
		print ("Vida sincronizada: " + _currentHealth);
		if (isLocalPlayer)
			A.text = "Vida: " + _currentHealth;
		else
			B.text = "Vida: " + _currentHealth;
	}

}
