using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlovePickup : MonoBehaviour {

	void OnTriggerEnter2D() {
		GameManager.instance.hasGloves = true;
		Destroy (this.gameObject);

		GameManager.instance.CompleteQuest (0);
	}
}
