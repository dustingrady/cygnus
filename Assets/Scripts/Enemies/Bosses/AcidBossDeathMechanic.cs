using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBossDeathMechanic : MonoBehaviour {
	private bool protocalOn = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name == "Protocol Override") {
			Debug.Log ("Hit by item!");
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.name == "Protocol Override") {
			Debug.Log ("Hit by item!");
		}
	}
}
