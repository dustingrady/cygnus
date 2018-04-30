using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

	[SerializeField]
	private Door door;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player") {
			GameObject cm = GameObject.Find("FollowCM");
			cm.GetComponent<CameraController> ().FocusForTime(door.gameObject, 2.5f);
			door.Open ();
		}
	}
}
