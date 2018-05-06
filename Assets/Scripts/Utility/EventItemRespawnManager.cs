using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventItemRespawnManager : MonoBehaviour {
	bool atCheckPoint = false;
	RespawnManager rm;
	GameObject prevEventItem;
	public GameObject Timer;
	// Use this for initialization
	void Start () {
		rm = GameObject.Find ("RespawnManager").GetComponent<RespawnManager> ();
		Debug.Log (rm.gameObject.name);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			if (!atCheckPoint) {
				if (rm.destructibles.Count > 0) {
					foreach (GameObject go in rm.destructibles) {
						//if it is destroyed before checkpoint, remove it from list
						if (!go.activeInHierarchy) {
							prevEventItem = go;
						}
					}
				}
				rm.removeDestItem (prevEventItem);
				atCheckPoint = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			rm.currentTime = Timer.GetComponent<Timer> ().timeLeft;
		}
	}
}
