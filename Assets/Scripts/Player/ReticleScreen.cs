using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleScreen : MonoBehaviour {
	public float reticleDistance = 1.5f;
	GameObject plr;

	void Start () {
		plr = GameObject.FindGameObjectWithTag ("Player");
	}

	// Update is called once per frame
	void Update () {
		if (GameManager.instance.controllerConnected == false) {
			// Get the location of the mouse relative to the player
			transform.position = Input.mousePosition;
		} else {
			float x = Input.GetAxis ("RightHorizontal");
			float y = Input.GetAxis ("RightVertical");

			if (Mathf.Abs (x) > 0.4 || Mathf.Abs (y) > 0.4) {
				Vector3 dir = new Vector3 (x, -y, 0).normalized;
				transform.position = Camera.main.WorldToScreenPoint(plr.transform.position + (dir * reticleDistance));
				gameObject.transform.localScale = Vector3.zero;

			} else {
				transform.position = plr.transform.position;
				gameObject.transform.localScale = Vector3.one * 20;

			}
		}
	}

	public void DisableReticle() {
		gameObject.GetComponent<Image> ().color = new Color (1, 1, 1, 0);
	}

	public void EnableReticle() {
		gameObject.GetComponent<Image> ().color = new Color (1, 1, 1, 1);
	}
}
