using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Money : NetworkBehaviour {

	public const int initialMoney = 0;

	[SyncVar(hook = "hook_CurrentMoney")]
	public int currentMoney = initialMoney;

	public void GainMoney(int amount) {
		if (!isServer)
			return;

		currentMoney += amount;
		print ("CUIDADO!!:  " + currentMoney);
		if (currentMoney == 0) {
			print ("Eres pobre");
		}
	}

	void hook_CurrentMoney(int _currentMoney) {
		CurrentMoney (_currentMoney);
	}

	protected virtual void CurrentMoney(int _currentMoney){
		// print ("Vida sincronizada: " + _currentHealth);
	}

}
