﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steam : Element {
	public GameObject burst;
	[SerializeField]
	private float burstStrength = 2000f;
	[SerializeField]
	private float burstCooldown = 1.5f;
	private float timeSinceFire;
	private bool btnReleased = true;

	public override void UseElement(Vector3 pos, Vector2 dir){
		if (timeSinceFire > burstCooldown && btnReleased) {
			GameObject steamObject = Instantiate (burst, pos, Quaternion.identity);

			// Change the angle to match the direction.
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			steamObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			//steamObject.transform.parent = transform.root;
			
			steamObject.GetComponent<ParticleSystem> ().Play ();

			transform.root.GetComponent<Rigidbody2D> ().AddForce (-dir.normalized * burstStrength);
			timeSinceFire = 0;
			btnReleased = false;
		}
	}

	void Update() {
		timeSinceFire += Time.deltaTime;

		if (Input.GetMouseButtonUp (2) == true
			|| Input.GetButtonUp("RightStick")) {
			btnReleased = true;
		}
	}
}
