using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InGameMenuEventTrigger : EventTrigger {

	GameObject gamemanager;

	// Use this for initialization
	void Start () {
		gamemanager  = GameObject.Find ("Game Manager");
	}

	public override void OnPointerClick(PointerEventData data)
	{
		Debug.Log (data);
	}

	public override void OnMove(AxisEventData data)
	{
		Debug.Log ("TEST");
	}
}
