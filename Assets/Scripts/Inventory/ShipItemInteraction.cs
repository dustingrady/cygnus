using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipItemInteraction : MonoBehaviour {

	public int questID;

	void OnTriggerEnter2D(Collider2D col) {
		GameManager.instance.CompleteQuest (questID);
		Debug.Log ("Completed quest: " + questID);
		Destroy (gameObject);
	}
}
