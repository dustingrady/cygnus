using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlovePickup : MonoBehaviour {

	public string glovesMsg;

	void OnTriggerEnter2D() {
		GameManager.instance.hasGloves = true;

		GameObject ui = GameObject.Find ("UI");
		ElementUI eleUI = ui.GetComponent<ElementUI> ();
		eleUI.EnableElements ();

		// Display some dialog
		DialogPopupController.Initialize();
		DialogPopupController.CreateDialogPopup(glovesMsg, 10f);

		Destroy (this.gameObject);

		GameManager.instance.CompleteQuest (0);
	}
}
