using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBarrier : MonoBehaviour {
	public bool godlike = true;

	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.gameObject.tag == "EMP") {
			godlike = false;
			DestroyObject (this.gameObject);
		}
	}
}