using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour {

	[SerializeField]
	private GameObject door;
	[SerializeField]
	private GameObject boss;
	[SerializeField]
	private GameObject switchTilemap;
	public bool bossAlive = true;

	void Start() {
		door.GetComponent<SpriteRenderer> ().enabled = false;
		door.GetComponent<BoxCollider2D> ().enabled = false;

		boss.SetActive (false);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.CompareTag ("Player")) {
			door.GetComponent<SpriteRenderer> ().enabled = true;
			door.GetComponent<BoxCollider2D> ().enabled = true;

			if (boss != null) {
				boss.SetActive (true);
				GetComponent<BoxCollider2D> ().enabled = false;
			}
		}
	}

	void Update() {
		if (boss.GetComponent<Enemy> ().getHP () <= 0 && bossAlive) {
			bossAlive = false;

			door.GetComponent<SpriteRenderer> ().enabled = false;
			door.GetComponent<BoxCollider2D> ().enabled = false;

			switchTilemap.SetActive (false);
		}
	}
}
