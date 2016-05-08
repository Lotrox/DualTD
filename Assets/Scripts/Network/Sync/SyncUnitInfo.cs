using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SyncUnitInfo : NetworkBehaviour {

	[SyncVar]
	float damage;
	[SyncVar]
	float health;

	public void setParams(float _damage, float _health){
		damage = _damage;
		health = _health;
	}

	public override void OnStartClient()
	{
		GetComponent<UnitInfo> ().damage += damage;
		GetComponent<UnitInfo> ().health += health;
		GetComponent<UnitInfo> ().max_health += health;
	}

}
