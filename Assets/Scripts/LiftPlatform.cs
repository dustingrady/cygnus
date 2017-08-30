using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftPlatform : MonoBehaviour {

	[SerializeField]
	private GameObject target;
	[SerializeField]
	private GameObject start;
	private bool movingToTarget;
	private bool movingToStart;
	private bool atStart;

	public float speed;

	void Start() {
		movingToTarget = false;
		movingToStart = false;
		atStart = true;
	}

	void Update() {
		if (movingToStart) {
			MoveToStart ();
		} else if (movingToTarget) {
			MoveToTarget ();
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		// Check if the platform is below the player's feet
		if ((col.transform.position.y - col.gameObject.GetComponent<BoxCollider2D> ().bounds.extents.y) > transform.position.y) {

			// The player jumps onto the top of the block, move up to the target
			if (!movingToTarget && atStart) {
				Debug.Log("Moving to target");
				movingToTarget = true;
			}
		}
	}

	void MoveToTarget() {
		transform.position = Vector2.Lerp (transform.position, target.transform.position, speed * Time.deltaTime);

		// Block has reached the target, move back to start
		if (Vector2.Distance (transform.position, target.transform.position) < 0.05) {
			// Creates the conditions to move back to start
			atStart = false;
			movingToStart = true;
		}
	}

	void MoveToStart() {
		transform.position = Vector2.Lerp (transform.position, start.transform.position, speed * Time.deltaTime);

		if (Vector2.Distance (transform.position, start.transform.position) < 0.02) {
			movingToTarget = false;
			movingToStart = false;
			atStart = true;
		}
	}
}
