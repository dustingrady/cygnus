//drag the hidden room into the reference.
//works as pair so each room will need its own trigger

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenRoom : MonoBehaviour {

	public GameObject room;

	public GameObject blockade;

	bool fading = false;
	Material m;
	Material mm;
	Transform[] t;

	void Start()
	{
		mm = room.GetComponent<TilemapRenderer> ().material;
		mm.color = new Color(mm.color.r, mm.color.g, mm.color.b, 0);
		if(blockade != null)
		t = blockade.transform.GetComponentsInChildren<Transform> ();
	}

	void Update()
	{
		if (fading && !room.GetComponent<TilemapCollider2D> ().enabled) {
			if (blockade != null) {
				for (int i = 1; i <= blockade.transform.childCount; i++) {
					if (t [i] != null) {
						m = t [i].gameObject.GetComponent<Renderer> ().material;
						if (m.color.a >= 0) {
							m.color = new Color (m.color.r, m.color.g, m.color.b, Mathf.Lerp (m.color.a, 0, Time.deltaTime * 4f));
						}
					}
				}

				if (m.color.a <= 0.01) {
					for (int i = 1; i <= blockade.transform.childCount; i++) {
						t [i].gameObject.SetActive (false);
					}

					room.GetComponent<TilemapRenderer> ().enabled = true;
					if (mm.color.a <= 1) {
						mm.color = new Color (mm.color.r, mm.color.g, mm.color.b, Mathf.Lerp (mm.color.a, 1, Time.deltaTime * 4f));
					}

					if (mm.color.a >= 0.99) {
						room.GetComponent<TilemapCollider2D> ().enabled = true;
						fading = false;
					}
				}
			} else {
				room.GetComponent<TilemapRenderer> ().enabled = true;
				if (mm.color.a <= 1) {
					mm.color = new Color (mm.color.r, mm.color.g, mm.color.b, Mathf.Lerp (mm.color.a, 1, Time.deltaTime * 4f));
				}
				if (mm.color.a >= 0.99) {
					room.GetComponent<TilemapCollider2D> ().enabled = true;
					fading = false;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player") {
			fading = true;
		}
	}
}
