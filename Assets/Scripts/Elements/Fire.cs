using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Element {
	public GameObject fireball;
	[SerializeField]
	private float fireballCooldown = 0.2f;
	private float timeSinceFire;
	private bool fireReleased = true;

	// Checks for controller release
	PlayerShooting plrs;

	public override void UseElement(Vector3 pos, Vector2 dir){
		if (timeSinceFire > fireballCooldown && fireReleased) {
			Vector3 spawnPos = new Vector3 (dir.normalized.x, dir.normalized.y, 0) * 0.8f;
			GameObject fb = Instantiate (fireball, pos + spawnPos, Quaternion.identity);
			fb.GetComponent<Fireball> ().Initialize (dir, 14);
			timeSinceFire = 0;
			fireReleased = false;
		}
	}

	void Start() {
		plrs = transform.root.GetComponent<PlayerShooting> ();
	}

	void Update() {
		timeSinceFire += Time.deltaTime;

		// UGLY
		if (GameManager.instance.controllerConnected) {
			if (!plrs.leftFireDown && !plrs.rightFireDown) {
				fireReleased = true;
			}
		} else {
			if (Input.GetMouseButtonUp (0) == true || Input.GetMouseButtonUp (1) == true) {
				fireReleased = true;
			}
		}
	}
}
