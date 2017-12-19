using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : Element {
	public GameObject Hook;
	[SerializeField]
	private bool metalReleased = true;

	public override void UseElement(Vector3 pos, Vector2 dir){
		if (metalReleased) {
			GameObject fb = Instantiate (Hook, pos, Quaternion.identity);
			fb.GetComponent<Magnet> ().Initialize (dir, 8);
			metalReleased = false;
		}
	}

	void Update() {
		if (Input.GetMouseButtonUp (0) == true || Input.GetMouseButtonUp (1) == true ) {
			metalReleased = true;
		}
	}
}