using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour {
	public bool playerCam = true;
	private GameObject bossRoomCM;
	private GameObject playerCM;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			playerCam = !playerCam;
		}
	}

	// Use this for initialization
	void Start () {
		bossRoomCM = GameObject.Find("BossRoomCamera");
		playerCM = GameObject.Find ("Follow CM");
	}
	
	// Update is called once per frame
	void Update () {
		if (playerCam == false) {
			playerCM.SetActive (false);
			bossRoomCM.SetActive (true);
		} else if (playerCam == true) {
			bossRoomCM.SetActive (false);
			playerCM.SetActive (true);
		}
	}
}
