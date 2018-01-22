using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlovePickup : MonoBehaviour {

	void OnTriggerEnter2D() {
		GameManager.instance.hasGloves = true;

		GameObject ui = GameObject.Find ("UI");
		ElementUI eleUI = ui.GetComponent<ElementUI> ();
		eleUI.EnableElements ();

		Destroy (this.gameObject);

		GameManager.instance.CompleteQuest (0);
	}
}
