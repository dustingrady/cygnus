using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour {
	
	public List<GameObject> activeEnemies = new List<GameObject>();
	public List<GameObject> hiddenEnemies = new List<GameObject>();

	//this is for my timer thing
	public List<GameObject> destructibles = new List<GameObject>();
	public float currentTime;
	public GameObject Timer;

	//this is for lifts only
	public List<GameObject> interactibleObjects = new List<GameObject>();

	void OnEnable() {
		Player.OnDeath += Respawn;
	}

	void OnDisable() {
		Player.OnDeath -= Respawn;
	}

	public void Respawn() {
		Debug.Log ("respawning enemies");

		if (activeEnemies.Count > 0) {
			foreach (GameObject go in activeEnemies) {
				go.GetComponent<Enemy> ().respawnActive ();
			}
		}

		if (hiddenEnemies.Count > 0) {
			foreach (GameObject go in hiddenEnemies) {
				go.GetComponent<Enemy> ().respawnHidden ();
				if (go.activeInHierarchy) {
					go.SetActive (false);
				}
			}
		}

		if (destructibles.Count > 0) {
			foreach (GameObject go in destructibles) {
				go.GetComponent<TimerEvent> ().resetItem ();
				go.SetActive (true);
			}
		}

		if (interactibleObjects.Count > 0) {
			foreach (GameObject go in interactibleObjects) {
				go.GetComponent<ElectricLift> ().reset ();
			}
		}

		if (Timer != null) {
			Timer.GetComponent<Timer> ().timeLeft = currentTime;
		}
	}

	public void removeDestItem(GameObject go){
		destructibles.Remove (go);
	}
}
