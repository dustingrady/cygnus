using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFireTrigger : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("WaterElement")) {
			GameManager.instance.CompleteQuest (1);
			Destroy (this.gameObject);
		}
	}
}
