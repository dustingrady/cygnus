using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {
	[SerializeField]
	private GameObject playerBullet;

	
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Shoot ();
		}
	}

	void Shoot() {
		if (transform.localScale.x > 0) {
			GameObject blt = Instantiate (playerBullet, transform.position, transform.rotation);
		} else {
			GameObject blt = Instantiate (playerBullet, transform.position, transform.rotation);
			blt.GetComponent<PlayerBullet> ().movingRight = false;;
		}
	}
}
