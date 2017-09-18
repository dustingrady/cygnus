using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {

	public float speed = 0.2f;
	public bool movingRight = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		if (movingRight) {
			transform.position += Vector3.right * speed;
		} else {
			transform.position += Vector3.left * speed;
			transform.localScale = new Vector3 (-1, 1, 1);
		}
	}
}
