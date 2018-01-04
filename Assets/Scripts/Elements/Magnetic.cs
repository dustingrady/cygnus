using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetic : Element {
	public float magnetStrength = 1000f;

	public LineRenderer lr;

	[SerializeField]
	private float magnetCooldown = 0.2f;
	private float timeSinceFire;
	private bool fireReleased = true;
	public bool pulling = false;
	Vector3 pullPos;

	public override void UseElement(Vector3 pos, Vector2 dir) {
		if (timeSinceFire > magnetCooldown && fireReleased) {

			RaycastHit2D hit = Physics2D.Raycast (pos, dir, 100, 1 << LayerMask.NameToLayer ("Ground"));

			if (hit.collider != null) {
				if (hit.collider.CompareTag ("MetalElement")) {
					pullPos = hit.point;
					pulling = true;
				}
			}

			timeSinceFire = 0;
			fireReleased = false;
		} else if (pulling) {
			
			// Get the transform of the player
			GameObject plr = gameObject.transform.root.gameObject;

			// Find the position difference so you know what direction to apply the force
			Vector3 diff = pullPos - plr.transform.position;

			// Make the force stronger depending on how far away from the object the player is located
			plr.GetComponent<Rigidbody2D> ().AddForce ((magnetStrength / diff.magnitude) * diff.normalized);

			// Enable the line renderer
			lr.enabled = true;

			// Update the line renderer
			lr.SetPositions (new Vector3[] { plr.transform.position + Vector3.back, pullPos + Vector3.back });

		}
	}

	void Start() {
		lr = GetComponent<LineRenderer> ();
	}

	void Update() {
		timeSinceFire += Time.deltaTime;

		// UGLY
		if (Input.GetMouseButtonUp (2) == true) {
			fireReleased = true;
			pulling = false;

			// Disable the line renderer
			lr.enabled = false;
		}
	}
}
