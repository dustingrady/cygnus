using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Element {
	public GameObject fireball;

	public override void UseElement(Vector3 pos, Vector2 dir){
		GameObject fb = Instantiate (fireball, pos, Quaternion.identity);
		fb.GetComponent<Fireball> ().Initialize (dir, 10);
	}
}
