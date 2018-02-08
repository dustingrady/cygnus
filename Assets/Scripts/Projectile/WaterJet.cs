﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterJet : MonoBehaviour {
	public Vector3 direction;

	public void Initialize(Vector2 direction, float speed) {
		this.direction = direction.normalized;
		gameObject.GetComponent<Rigidbody2D> ().AddForce (this.direction*speed);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag != "Player"
		    && col.gameObject.tag != "WaterElement"
		    && col.name != "Bounds") {

			if (col.gameObject.tag == "Enemy") {
				if (col.gameObject.GetComponent<PatrolType> () != null && col.gameObject.GetComponent<PatrolType> ().getEnemyType () == "fire") {
					col.gameObject.GetComponent<PatrolType> ().takeDamage (2);
				}

				if (col.gameObject.GetComponent<TurretType> () != null && col.gameObject.GetComponent<TurretType> ().getEnemyType () == "fire") {
					col.gameObject.GetComponent<TurretType> ().takeDamage (1);
				}
			}
		
			DestroyObject (this.gameObject);
		
		}
	}
}
