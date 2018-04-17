using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBarrier : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D col) {
		if (col.gameObject.gameObject.tag == "EMP") {

			GameObject papa = transform.root.gameObject;
			papa.GetComponent<DesertBoss> ().godLike = false;

			DestroyObject (this.gameObject);
		}
	}
}