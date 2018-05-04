using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTimerCheckpoint : MonoBehaviour {
	
	RespawnManager rm;

	public GameObject Timer;
	// Use this for initialization
	void Start () {
		rm = GameObject.Find ("RespawnManager").GetComponent<RespawnManager> ();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			if (rm.destructibles.Count > 0) {
				Timer.GetComponent<Timer> ().Active = false;
			}
		}
	}
}
