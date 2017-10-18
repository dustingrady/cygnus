using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Element {
	public GameObject waterJet;

	public override void UseElement(Vector3 pos, Vector2 dir){
		//Debug.Log ("Test");
		GameObject fb = Instantiate (waterJet, pos, Quaternion.identity);
		fb.GetComponent<WaterJet> ().Initialize (dir, 10);
	}
}