using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

	[SerializeField]
	private Door door;
	[SerializeField]
	private List<Door> bonusDoors;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player" && door != null) {
			GameObject cm = GameObject.Find("FollowCM");
			cm.GetComponent<CameraController> ().FocusForTime(door.gameObject, 2.5f);
			door.Open ();
		
			if (bonusDoors != null) {
				foreach (Door bonusDoor in bonusDoors) {
					bonusDoor.Open ();
				}
			}
		}
	}
}
