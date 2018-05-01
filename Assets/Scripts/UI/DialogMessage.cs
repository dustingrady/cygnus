using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogMessage : MonoBehaviour {

	public string message;
	public float duration;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.name == "Player") {
			DialogPopupController.Initialize ();
			DialogPopupController.CreateDialogPopup (message, duration);
			Destroy (gameObject);
		}
	}
}
