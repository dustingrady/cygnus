using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : Element {
	public GameObject metal;
	[SerializeField]
	private float boulderStrength = 500;
	[SerializeField]
	private float metalCooldown = 1.5f;
	private float timeSinceFire;
	private bool metalReleased = true;

	public override void UseElement(Vector3 pos, Vector2 dir){
		if (timeSinceFire > metalCooldown && metalReleased) {
			GameObject fb = Instantiate (metal, pos, Quaternion.identity);
			fb.GetComponent<Boulder> ().Initialize (dir, boulderStrength);
			timeSinceFire = 0;
			metalReleased = false;
		}
	}

	void Update() {
		timeSinceFire += Time.deltaTime;

		if (Input.GetMouseButtonUp (0) == true || Input.GetMouseButtonUp (1) == true ) {
			metalReleased = true;
		}
	}
}