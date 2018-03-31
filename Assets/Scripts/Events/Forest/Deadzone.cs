using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadzone : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.CompareTag ("Player")) {
			Player plr = col.gameObject.GetComponent<Player> ();
			plr.health.CurrentVal = 0;
		}
	}
}
