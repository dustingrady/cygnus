using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBreakable : Door {
	private float timeToDestroy = 3.0f;

	public override void Open() {
		Rigidbody2D[] chillins = GetComponentsInChildren<Rigidbody2D> ();
		foreach (Rigidbody2D chillin in chillins) {
			chillin.bodyType = RigidbodyType2D.Dynamic;
			chillin.AddForce (new Vector2 (Random.Range (-1000, 1000), 0));
			chillin.gameObject.transform.parent = null;
			StartCoroutine ("DestroyChillin", chillin.gameObject);
		}

		Destroy (gameObject, 1f);
	}


	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "EarthElement") {	
			Open ();
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "EarthElement") {
			Open ();
		}
	}

	IEnumerator DestroyChillin(GameObject chillin) {
		yield return new WaitForSeconds (0.9f);
		Destroy (chillin);
	}
}