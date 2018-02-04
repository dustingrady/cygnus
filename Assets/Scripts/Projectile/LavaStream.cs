using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaStream : MonoBehaviour {
	public Vector3 direction;

	public void Initialize(Vector2 direction, float speed) {
		this.direction = direction.normalized;
		gameObject.GetComponent<Rigidbody2D> ().AddForce (this.direction*speed);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag != "Player" 
			&& col.gameObject.tag != "Lava" 
			&& col.name != "Bounds") {
			DestroyObject (this.gameObject);
		}
	}
}
