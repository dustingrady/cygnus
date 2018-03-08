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

	void Update()
	{
		if (fading && !room.GetComponent<TilemapRenderer> ().enabled) {
			Transform[] t = blockade.transform.GetComponentsInChildren<Transform> ();
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

				fading = false;
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
