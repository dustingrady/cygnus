using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossEnable : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("Player")) {
			GameObject.Find ("Boss").GetComponent<DesertBoss> ().enabled = true;
			gameObject.SetActive (false);
		}
	}
}
