using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Absorbs different materials for player to use
public class Absorber : MonoBehaviour {

	public float speed;
	public Vector3 direction;
	private string hand;
	public AudioClip clip;

	ElementManager elementManager;

	// Event broadcast
	public delegate void Absorbed();
	public static event Absorbed OnAbsorb;

	public void Initialize(Vector2 direction, float speed, string hand) {
		this.speed = speed;
		this.direction = direction.normalized;
		this.hand = hand;
	}

	void Start() {
		AudioSource.PlayClipAtPoint (clip, this.transform.position);
		GameObject go = GameObject.FindGameObjectWithTag ("Player");
		elementManager = go.GetComponentInChildren<ElementManager> ();
	}

	void Update() {
		transform.position += direction * speed * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D col) {

		// Tell the element manager to assign the correct element to the correct hand
		// based on the tag of the element hit and the hand that threw it
		elementManager.AssignToHand (hand, col.gameObject.tag);


		// Destroy the absorber if the collider it hits isn't the player
		if (col.gameObject.tag != "Player") {
			DestroyObject (this.gameObject);

			// Broadcast that the absorber has collided with something
			// This should cause the UI to attempt an update
			if (OnAbsorb != null) {
				OnAbsorb ();
			}
		}

	}
}
