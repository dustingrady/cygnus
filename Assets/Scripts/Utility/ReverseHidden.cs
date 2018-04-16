using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ReverseHidden : MonoBehaviour {


	public GameObject room;
	public bool buttonPress = false;
	bool fading = false;
	Material m;

	void Start()
	{
		m = room.GetComponent<TilemapRenderer> ().material;
	}

	void Update()
	{
		if (buttonPress) {
			if (Input.GetKeyDown (KeyCode.E)) {
				buttonPress = false;
			}
		} else {
			fadeTrigger ();
		}
	}

	void fadeTrigger()
	{
		if (fading) {
			if (m.color.a >= 0) {
				m.color = new Color (m.color.r, m.color.g, m.color.b, Mathf.Lerp (m.color.a, 0, Time.deltaTime * 4f));
			}
			if (m.color.a <= 0.01) {
				if (room.GetComponent<TilemapCollider2D> () != null) {
					room.GetComponent<TilemapCollider2D> ().enabled = false;
				}

				fading = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player") {
			fading = true;
			FloatingTextController.CreateFloatingText ("Hidden Room Uncovered!", this.gameObject.transform, Color.blue, 20);
		}
	}
}
