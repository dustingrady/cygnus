using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogMessages : MonoBehaviour {

	public List<string> messages;
	public List<float> durations;
	private bool activated = false;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.name == "Player" && activated == false) {
			StartCoroutine (showDialog (0));
		}

		activated = true;
	}

	IEnumerator showDialog(int index) {
		DialogPopupController.Initialize ();
		DialogPopupController.CreateDialogPopup (messages[index], durations[index]);
		yield return new WaitForSeconds (durations[index]);
		if (index + 1 < durations.Count) {
			StartCoroutine (showDialog (index + 1));
		}
	}
}
