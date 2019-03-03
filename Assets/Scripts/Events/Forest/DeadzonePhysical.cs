using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadzonePhysical : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D col) {
		if (col.collider.gameObject.CompareTag ("Player")) {
			Player plr = col.gameObject.GetComponent<Player> ();
			plr.health.CurrentVal = 0;
		}
	}
}
