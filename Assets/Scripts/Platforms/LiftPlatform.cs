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
	[SerializeField]
	private bool autoReset;

	public float speed;

	void Start() {
		movingToTarget = false;
		movingToStart = false;
		atStart = true;
	}

	void FixedUpdate() {
		if (movingToStart) {
			MoveToStart ();
		} else if (movingToTarget) {
			MoveToTarget ();
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		// Check if the platform is below the player's feet
		if (col.gameObject.name == "Player") {
			if ((col.transform.position.y - col.gameObject.GetComponent<BoxCollider2D> ().bounds.extents.y) > transform.position.y) {

				// Attatch the player to the platform by parenting them
				col.transform.parent = transform;

				// The player jumps onto the top of the block, move up to the target
				if (!movingToTarget && atStart) {
					//Debug.Log("Moving to target");
					movingToTarget = true;
				}
			}
		}
	}

	void OnCollisionExit2D(Collision2D col) {
		// Detatch the player
		col.transform.parent = null;

		// Player left, go back to start!
		movingToStart = true;

		// Work out the velocity for the player leaving the platform
		Vector2 platformVel = Vector2.zero;

		// Get the direction of the movement
		Vector3 dir = target.transform.position - start.transform.position;
		dir = dir.normalized;


		if (movingToStart) {
			platformVel = new Vector2 (-speed * dir.x, -speed * dir.y);
		} else if (movingToTarget) {
			platformVel = new Vector2 (speed * dir.x, speed * dir.y);
		} 

		Rigidbody2D plrRb = col.gameObject.GetComponent<Rigidbody2D> ();
		plrRb.velocity = plrRb.velocity + platformVel;

	}

	void MoveToTarget() {
        //Varying speed vs constant speed
        //transform.position = Vector2.Lerp (transform.position, target.transform.position, speed * Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 0.05f * speed);

		// Check for autoReset, if you get close to the end with it set, go back
		if (Vector2.Distance (transform.position, target.transform.position) < 0.05 && autoReset) {
			// Creates the conditions to move back to start
			atStart = false;
			movingToStart = true;
		}
	}

	void MoveToStart() {
        //Varying speed vs constant speed
        //transform.position = Vector2.Lerp (transform.position, start.transform.position, speed * Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, start.transform.position, 0.05f * speed);

        if (Vector2.Distance (transform.position, start.transform.position) < 0.02) {
			movingToTarget = false;
			movingToStart = false;
			atStart = true;
		}
	}
}
