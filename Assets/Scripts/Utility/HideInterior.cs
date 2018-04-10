using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HideInterior : MonoBehaviour {

	[SerializeField]
	private bool hidden = false;
	private TilemapRenderer tmr;

	// Use this for initialization
	void Start () {
		tmr = GetComponent<TilemapRenderer> ();

		if (hidden) {
			tmr.enabled = true;
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Player")) {
			hidden = false;
			tmr.enabled = false;
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.CompareTag ("Player")) {
			hidden = true;
			tmr.enabled = true;
		}
	}
}
