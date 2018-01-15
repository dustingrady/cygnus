using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Boulder : MonoBehaviour {
	float speed;
	public Vector3 direction;

	public void Initialize(Vector2 direction, float speed) {
		this.direction = direction.normalized;
		gameObject.GetComponent<Rigidbody2D> ().AddForce (this.direction*speed);
	}
	/*
	void Update() {
		transform.position += direction * speed * Time.deltaTime;
	}*/

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag != "Player" 
			&& col.gameObject.tag != "Boulder" 
			&& col.name != "Bounds") {
			DestroyObject (this.gameObject);
		}
	}
}
