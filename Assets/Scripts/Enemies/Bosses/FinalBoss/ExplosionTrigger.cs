using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour {
	[SerializeField]
	private GameObject leftFlame;
	[SerializeField]
	private GameObject rightFlame;
	[SerializeField]
	private GameObject explosionParticles;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag("Player")) {
			Debug.Log("gotcha...");
			Explode ();
		}
	}

	void Explode() {
		GameObject plr = GameManager.instance.player.gameObject;
		plr.GetComponent<Rigidbody2D> ().AddForce (new Vector2(1000f, 1000f));
		Instantiate (explosionParticles, new Vector3(transform.position.x, transform.position.y - 3, 1), Quaternion.identity);

		leftFlame.SetActive (true);
		rightFlame.SetActive (true);

		Player.OnDeath += ResetTrigger;

		this.gameObject.SetActive (false);
	}

	void ResetTrigger() {
		Debug.Log ("resetting");
		this.gameObject.SetActive (true);
		leftFlame.SetActive (false);
		rightFlame.SetActive (false);
	}

}
