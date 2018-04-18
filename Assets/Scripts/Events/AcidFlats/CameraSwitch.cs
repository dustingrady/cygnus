using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour {
	public bool playerCam = true;
	private GameObject targetCM;
	private GameObject playerCM;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			playerCam = false;
		}
	}

	// Use this for initialization
	void Start () {
		targetCM = GameObject.Find("BossCamera");
		playerCM = GameObject.Find ("Follow CM");
	}
	
	// Update is called once per frame
	void Update () {
		if (playerCam == false && GameObject.Find("snek") != null) {
			playerCM.SetActive (false);
			targetCM.SetActive (true);
		} else if (playerCam == true) {
			targetCM.SetActive (false);
			playerCM.SetActive (true);
		}
	}
}
