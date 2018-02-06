using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour {
	public float reticleDistance = 1.5f;
	GameManager gm;
	GameObject plr;

	void Start () {
		// Get the game manager so we can check to see if a controller is connected
		gm = GameManager.instance;
		plr = GameObject.FindGameObjectWithTag ("Player");
	}

	void Update () {

	}

	void LateUpdate () {

		if (!gm.controllerConnected) {
			// Get the location of the mouse relative to the player
			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			transform.position = new Vector3 (mousePos.x, mousePos.y, 1);
		} else {
			float x = Input.GetAxis ("RightHorizontal");
			float y = Input.GetAxis ("RightVertical");

			if (Mathf.Abs (x) > 0.4 || Mathf.Abs (y) > 0.4) {
				Vector3 dir = new Vector3 (x, -y, 0).normalized;
				transform.position = plr.transform.position + (dir * reticleDistance);

				gameObject.GetComponent<SpriteRenderer> ().enabled = true;
			} else {
				transform.position = plr.transform.position;

				gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			}
		}
	}
}