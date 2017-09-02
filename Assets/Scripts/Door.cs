using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void destroy() {
		Rigidbody2D[] chillins = GetComponentsInChildren<Rigidbody2D> ();
		foreach (Rigidbody2D chillin in chillins) {
			chillin.bodyType = RigidbodyType2D.Dynamic;
			chillin.AddForce (new Vector2 (Random.Range (-1000, 1000), 0));
			chillin.gameObject.transform.parent = null;
		}
	}
}
