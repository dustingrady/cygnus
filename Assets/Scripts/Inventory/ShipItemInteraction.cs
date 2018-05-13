using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipItemInteraction : MonoBehaviour {

	public int questID;
	public string collectedText;

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Player")) {
			GameManager.instance.CompleteQuest (questID);
			Debug.Log ("Completed quest: " + questID);

			DialogPopupController.Initialize ();
			DialogPopupController.CreateDialogPopup (collectedText, 10);

			Destroy (gameObject);
		}
	}
}
