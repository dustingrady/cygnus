using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour {

	public float maxDist = 20f;

	// Use this for initialization
	void Start () {
		
	}

	void Update() {
		transform.Translate((Vector3.left * Time.deltaTime)/3);

		if (transform.localPosition.x < -22) {
			transform.Translate (Vector3.right * 44);
		}
	}
}
