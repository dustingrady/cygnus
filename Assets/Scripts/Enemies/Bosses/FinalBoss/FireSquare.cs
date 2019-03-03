using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSquare : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag("WaterElement")) {
			Debug.Log("COOLING DOWN A WATER TILE");
			gameObject.SetActive(false);
		}
	}
}
