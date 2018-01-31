﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fireball : MonoBehaviour {
	float speed;
	public Vector3 direction;

	public void Initialize(Vector2 direction, float speed) {
		this.speed = speed;
		this.direction = direction.normalized;

		// Change the angle to match the direction.
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	void Update() {
		transform.position += direction * speed * Time.deltaTime;
	}

	/*
	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag != "Player" && col.gameObject.tag != "Fireball" ) {
			DestroyObject (this.gameObject);
		}
	}*/

	void OnCollisionStay2D(Collision2D col)
	{
		//if (col.gameObject.tag != "Player" && col.gameObject.tag != "Fireball" ) {
			DestroyObject (this.gameObject);
		//}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Enemy") {
			if (col.gameObject.GetComponent<PatrolType> () != null && col.gameObject.GetComponent<PatrolType> ().getEnemyType() == "metal") {
				col.gameObject.GetComponent<PatrolType> ().takeDamage (10);
			}

			if (col.gameObject.GetComponent<TurretType> () != null && col.gameObject.GetComponent<TurretType> ().getEnemyType() == "metal") {
				col.gameObject.GetComponent<TurretType> ().takeDamage (5);
			}
			DestroyObject (this.gameObject);
		}

	}
}
