using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterJet : MonoBehaviour {
	public Vector3 direction;
	public Rigidbody2D rb;
	public AudioClip clip;

	public void Initialize(Vector2 direction, float speed) {
		this.direction = direction.normalized;
		gameObject.GetComponent<Rigidbody2D> ().AddForce (this.direction*speed);

		rb = GetComponent<Rigidbody2D> ();
	}

	void Start(){
		AudioSource.PlayClipAtPoint (clip, this.transform.position);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag != "Player" && col.gameObject.tag != "WaterElement") {
			/*
			if (col.gameObject.tag == "Enemy") {
				if (col.gameObject.GetComponent<PatrolType> () != null && col.gameObject.GetComponent<PatrolType> ().getEnemyType () == "fire") {
					col.gameObject.GetComponent<PatrolType> ().takeDamage (2);
				}

				if (col.gameObject.GetComponent<TurretType> () != null && col.gameObject.GetComponent<TurretType> ().getEnemyType () == "fire") {
					col.gameObject.GetComponent<TurretType> ().takeDamage (1);
				}
			}
		*/
			DestroyObject (this.gameObject);
		
		}
	}

	void Update() {
		Vector2 v = rb.velocity;
		float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
