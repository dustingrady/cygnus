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
		if (col.gameObject.tag == "Enemy") {
			if (col.gameObject.GetComponent<PatrolType> () != null && col.gameObject.GetComponent<PatrolType> ().getEnemyType () == "earth") {
				col.gameObject.GetComponent<PatrolType> ().takeDamage (10);
			}

			if (col.gameObject.GetComponent<TurretType> () != null && col.gameObject.GetComponent<TurretType> ().getEnemyType () == "earth") {
				col.gameObject.GetComponent<TurretType> ().takeDamage (5);
			}

		}

		DestroyObject (this.gameObject, 0.01f);
	}
}
