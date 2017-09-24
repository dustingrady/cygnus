using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
	[SerializeField]
	private GameObject absorber;
	[SerializeField]
	private float shootCooldown = 0.25f;
	[SerializeField]
	private float absorberSpeed = 10f;
	[SerializeField]
	private float absorberCooldown = 0.25f;
	private float shootTimer = 0f;
	private float absorbTimer = 0f;

	public delegate void ShootAction();
	public static event ShootAction OnShoot;

	void Update () {
		shootTimer += Time.deltaTime;
		absorbTimer += Time.deltaTime;

		if (Input.GetMouseButtonDown(0) && shootTimer > shootCooldown) {
			// Shoot goes here
		}

		if (Input.GetMouseButtonDown(1) && absorbTimer > absorberCooldown) {
			Absorb ();

			// Broadcast the shoot event to anyone who cares (PlayerAnimations)
			if (OnShoot != null) {
				OnShoot ();
			}
		}

	}

	void Shoot() {
		
	}

	void Absorb() {
		// Get the location of the mouse relative to the player
		Vector3 dirV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

		// Convert this into a Vector2
		Vector2 dir = new Vector2 (dirV3.x, dirV3.y);

		// Instantiate the Sorb Orb with a direction and speed
		GameObject blt = Instantiate (absorber, transform.position, transform.rotation);
		blt.GetComponent<Absorber> ().Initialize (dir, absorberSpeed);

		// restart the clock on the shoot cooldown
		absorbTimer = 0f;
	}
}
