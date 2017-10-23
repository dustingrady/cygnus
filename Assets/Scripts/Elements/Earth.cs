using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : Element {
	public GameObject earth;
	[SerializeField]
	private float earthCooldown = 0.2f;
	private float timeSinceFire;
	private bool earthReleased = true;

	public override void UseElement(Vector3 pos, Vector2 dir){
		if (timeSinceFire > earthCooldown && earthReleased) {
			GameObject fb = Instantiate (earth, pos, Quaternion.identity);
			fb.GetComponent<Boulder> ().Initialize (dir, 8);
			timeSinceFire = 0;
			earthReleased = false;
		}
	}

	void Update() {
		timeSinceFire += Time.deltaTime;

		// UGLY
		if (Input.GetMouseButtonUp (0) == true || Input.GetMouseButtonUp (1) == true ) {
			earthReleased = true;
		}
	}
}