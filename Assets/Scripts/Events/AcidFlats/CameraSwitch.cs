using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour {
	public bool playerCam = true;
	private GameObject bossCM;
	private GameObject playerCM;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			playerCam = false;
		}
	}

	// Use this for initialization
	void Start () {
		bossCM = GameObject.Find("BossCamera");
		playerCM = GameObject.Find ("Follow CM");
	}
	
	// Update is called once per frame
	void Update () {
		if (playerCam == false && GameObject.Find("AcidBoss") != null) {
			playerCM.SetActive (false);
			bossCM.SetActive (true);
		} else if (playerCam == true) {
			bossCM.SetActive (false);
			playerCM.SetActive (true);
		}
	}
}
