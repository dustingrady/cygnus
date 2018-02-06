using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterJet : MonoBehaviour {
	public Vector3 direction;
	[SerializeField]

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
			&& col.gameObject.tag != "WaterElement" 
			&& col.name != "Bounds") {
			DestroyObject (this.gameObject);
		}
	}*/

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Enemy") {
			if (col.gameObject.GetComponent<PatrolType> () != null && col.gameObject.GetComponent<PatrolType> ().getEnemyType() == "fire") {
				col.gameObject.GetComponent<PatrolType> ().takeDamage (2);
			}

			if (col.gameObject.GetComponent<TurretType> () != null && col.gameObject.GetComponent<TurretType> ().getEnemyType() == "fire") {
				col.gameObject.GetComponent<TurretType> ().takeDamage (1);
			}
		}
		DestroyObject (this.gameObject, 0.01f);
	}
}
