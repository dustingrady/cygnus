using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinking : MonoBehaviour {
	SpriteRenderer sr;
	float blinkTimer = 0f;
	ElectricLift el;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		el = GetComponentInParent<ElectricLift> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!el.electrifying) {
			flashing ();
		} else {
			sr.color = Color.green;
		}
	}

	void flashing()	{
		if (blinkTimer >= 1f) {
			sr.color = new Color (1f, 0f, 0f);
		}

		if (blinkTimer < 1f) {
			sr.color = Color.white;
		}

		blinkTimer += Time.deltaTime;

		if (blinkTimer >= 2f) {
			blinkTimer = 0;
		}
	}
}
