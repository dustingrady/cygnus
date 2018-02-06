using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaStream : MonoBehaviour {
	public Vector3 direction;

	public void Initialize(Vector2 direction, float speed) {
		this.direction = direction.normalized;
		gameObject.GetComponent<Rigidbody2D> ().AddForce (this.direction*speed);
	}

	void OnCollisionEnter2D(Collision2D col) {
			DestroyObject (this.gameObject, 0.01f);
	}
}
