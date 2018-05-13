using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogMessage : MonoBehaviour {

	public string message;
	public float duration;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.name == "Player") {
			// Destroy the old window if one exists
			GameObject previousWindow = GameObject.Find("DialogueDisplay(Clone)");
			if (previousWindow != null) {
				Destroy (previousWindow);
			}

			DialogPopupController.Initialize ();
			DialogPopupController.CreateDialogPopup (message, duration);
			Destroy (gameObject);
		}
	}
}
