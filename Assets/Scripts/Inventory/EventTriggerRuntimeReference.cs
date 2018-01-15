using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerRuntimeReference : EventTrigger {
	GameObject gamemanager;

	// Use this for initialization
	void Start () {
		gamemanager = GameObject.Find ("Game Manager");
	}

	public override void OnPointerEnter(PointerEventData data)
	{
		gamemanager.GetComponent<Inventory> ().showToolTip (this.gameObject);
		gamemanager.GetComponent<Inventory> ().checkSlot (this.gameObject);
	}

	public override void OnPointerExit(PointerEventData data)
	{
		gamemanager.GetComponent<Inventory> ().hideToolTip ();
		gamemanager.GetComponent<Inventory> ().checkExit (this.gameObject);
	}

	public override void OnBeginDrag(PointerEventData data)
	{
		gamemanager.GetComponent<Inventory> ().moveItem (this.gameObject);
	}

	public override void OnDrag(PointerEventData data)
	{
		gamemanager.GetComponent<Inventory> ().dragging ();
	}

	public override void OnEndDrag(PointerEventData data)
	{
		gamemanager.GetComponent<Inventory> ().stopDragging ();
	}
}
