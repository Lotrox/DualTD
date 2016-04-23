using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Health : NetworkBehaviour {

	public const int maxHealth = 15;

	[SyncVar(hook = "hook_CurrentHealth")]
	public int currentHealth = maxHealth;

	public void TakeDamage(int amount) {
		if (!isServer)
			return;

		currentHealth -= amount;

	}
		
	public bool IsDead(){
		if (isServer) {
			if (currentHealth == 0) {
				print ("Has muerto");
				return true;
			}
		}
		return false;
	}


	public int getHealth(){
		return currentHealth;
	}

	void hook_CurrentHealth(int _currentHealth) {
		CurrentHealth (_currentHealth);
	}

	protected virtual void CurrentHealth(int _currentHealth){
		// print ("Vida sincronizada: " + _currentHealth);
	}



}
