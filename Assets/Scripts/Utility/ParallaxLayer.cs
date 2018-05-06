using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour {

	GameObject plr;
	float plrX;

	// Use this for initialization
	void Start () {
		plr = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		plrX = plr.GetComponent<Rigidbody2D> ().velocity.x / 500;
		transform.position = new Vector3 (transform.position.x - plrX, transform.position.y, transform.position.z);
	}
}
