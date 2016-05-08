using UnityEngine;
using System.Collections;

public class TowerUI : MonoBehaviour {

	public GameObject tower;

	public void changeTower() {
		TowerSpawner.tower = tower;
		Camera.main.GetComponent<freeCam> ().set_aimCursor (true);
	}

	public void deleteTower() {
		TowerDeleter.readyForDelete = true;
		Camera.main.GetComponent<freeCam> ().set_aimCursor (true);
	}
}
