using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InGameMenuEventTrigger : EventTrigger {

	GameObject gamemanager;

	// Use this for initialization
	void Start () {
		gamemanager = GameObject.Find ("Game Manager");
	}

	public override void OnPointerClick(PointerEventData data)
	{
		switch (this.gameObject.name) {
		case "Resume":
			gamemanager.GetComponent<InGameMenu> ().Resume ();
			break;
		case "Save":
			gamemanager.GetComponent<InGameMenu> ().Save ();
			break;
		case "Load":
			gamemanager.GetComponent<InGameMenu> ().Load ();
			break;
		case "Quit":
			gamemanager.GetComponent<InGameMenu> ().Quit ();
			break;
		}

	}
}