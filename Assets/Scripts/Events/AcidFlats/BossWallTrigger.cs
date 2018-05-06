using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWallTrigger : MonoBehaviour {
	public GameObject wall;
	public bool wallOn = false;
	private AudioController ac;

	// Use this for initialization
	void Start () {
		//Reference to Audio Controller
		GameObject camera = GameObject.Find("Main Camera");
		ac = camera.GetComponent<AudioController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (wallOn == false || GameObject.Find ("AcidBoss") == null)
			wall.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			wall.SetActive(true);
			wallOn = true;

			if (ac.audio [1] != null) {
				ac.source.Stop ();
				ac.source.clip = ac.audio [1]; //Switch to boss music
				ac.source.Play ();
			}
		}
	}
}
