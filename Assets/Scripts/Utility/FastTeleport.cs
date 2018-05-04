using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTeleport : MonoBehaviour {

	public Vector3 pos;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Player")) {
			col.gameObject.transform.position = pos;
		}
	}
}