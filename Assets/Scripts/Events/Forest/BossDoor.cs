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
	private AudioController ac;

	void Start() {
		Player.OnDeath += ResetDoor;
		door.GetComponent<SpriteRenderer> ().enabled = false;
		door.GetComponent<BoxCollider2D> ().enabled = false;

		//Reference to Audio Controller
		GameObject camera = GameObject.Find("Main Camera");
		ac = camera.GetComponent<AudioController>();

		boss.SetActive (false);
	}

	void OnDestroy() {
		Player.OnDeath -= ResetDoor;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.CompareTag ("Player")) {
			door.GetComponent<SpriteRenderer> ().enabled = true;
			door.GetComponent<BoxCollider2D> ().enabled = true;

			if (boss != null) {
				boss.SetActive (true);
				GetComponent<BoxCollider2D> ().enabled = false;
				if (ac.audio [1] != null) {
					ac.source.Stop ();
					ac.source.clip = ac.audio [1]; //Switch to boss music
					ac.source.Play ();
				}
			}
		}
	}

	void Update() {
		if (boss.GetComponent<Enemy> ().getHP () <= 0 && bossAlive) {
			bossAlive = false;
			ac.source.clip = ac.audio [0]; //Switch back to default music after beating boss
			ac.source.Play (); 
			door.GetComponent<SpriteRenderer> ().enabled = false;
			door.GetComponent<BoxCollider2D> ().enabled = false;

			switchTilemap.SetActive (false);
		}
	}

	void ResetDoor() {
		Debug.Log ("opening the door again");
		// Open the door back up
		door.GetComponent<SpriteRenderer> ().enabled = false;
		door.GetComponent<BoxCollider2D> ().enabled = false;

		// Disable cave guardian script
		if (boss.GetComponent<CaveGuardian> ().activated == true) {
			boss.GetComponent<CaveGuardian>().respawned = true;
		}
		boss.SetActive (false);

		BossHealthBar healthBar = GameObject.Find("BossHealthBar").GetComponent<BossHealthBar>();
		healthBar.Disable();

		// Reenable the collider
		GetComponent<BoxCollider2D> ().enabled = true;
	}
}
