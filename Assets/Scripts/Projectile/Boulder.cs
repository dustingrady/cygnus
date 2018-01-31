﻿using System.Collections;
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
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag != "Player" 
			&& col.gameObject.tag != "Boulder" 
			&& col.name != "Bounds") {
			DestroyObject (this.gameObject);
		}
	}*/

	void OnCollisionStay2D(Collision2D col)
	{
		//if (col.gameObject.tag != "Player" 
		//	&& col.gameObject.tag != "Boulder" 
		//	&& col.gameObject.name != "Bounds") {
			DestroyObject (this.gameObject);
		//}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Enemy") {
			if (col.gameObject.GetComponent<PatrolType> () != null && col.gameObject.GetComponent<PatrolType> ().getEnemyType() == "water") {
				col.gameObject.GetComponent<PatrolType> ().takeDamage (5 + 2*this.gameObject.transform.localScale.x);
			}

			if (col.gameObject.GetComponent<TurretType> () != null && col.gameObject.GetComponent<TurretType> ().getEnemyType() == "water") {
				col.gameObject.GetComponent<TurretType> ().takeDamage (2 + 2*this.gameObject.transform.localScale.x);
			}

			DestroyObject (this.gameObject);
		}
	}
}
