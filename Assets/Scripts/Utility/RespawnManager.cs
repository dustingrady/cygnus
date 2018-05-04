using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour {

	GameObject player;

	bool respawn = false;

	public List<GameObject> activeEnemies = new List<GameObject>();
	public List<GameObject> hiddenEnemies = new List<GameObject>();

	//this is for my timer thing
	public List<GameObject> destructibles = new List<GameObject>();
	public float currentTime;
	public GameObject Timer;

	//this is for lifts only
	public List<GameObject> interactibleObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<Player> ().health.CurrentVal <= 0) {
			respawn = true;
		}

		if (respawn) {
			if (activeEnemies.Count > 0) {
				foreach (GameObject go in activeEnemies) {
					go.GetComponent<Enemy> ().respawnActive ();
				}
			}

			if (hiddenEnemies.Count > 0) {
				foreach (GameObject go in hiddenEnemies) {
					go.GetComponent<Enemy> ().respawnHidden ();
					Debug.Log (go.name);
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
			respawn = !respawn;
		}
	}

	public void removeDestItem(GameObject go){
		destructibles.Remove (go);
	}
}
