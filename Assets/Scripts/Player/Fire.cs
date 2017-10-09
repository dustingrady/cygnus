using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Absorber {
	//private float speed;
	//private Vector3 direction;

	public void Initialize(Vector2 direction, float speed) {
		this.speed = speed;
		this.direction = direction.normalized;
	}

	void Update() {
		transform.position += direction * speed * Time.deltaTime;
	}

	//Fire ability stuff here
	void useAbility(){

	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag != "Player") {
			DestroyObject (this.gameObject);
		}
	}
}
