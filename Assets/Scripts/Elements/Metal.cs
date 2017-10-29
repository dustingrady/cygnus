using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : Element {
	public GameObject metal;
	[SerializeField]
	private bool earthReleased = true;

	public override void UseElement(Vector3 pos, Vector2 dir){
		if (earthReleased) {
			GameObject fb = Instantiate (metal, pos, Quaternion.identity);
			fb.GetComponent<Boulder> ().Initialize (dir, 8);
			earthReleased = false;
		}
	}

	void Update() {
		if (Input.GetMouseButtonUp (0) == true || Input.GetMouseButtonUp (1) == true ) {
			earthReleased = true;
		}
	}
}