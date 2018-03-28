using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInput : MonoBehaviour {

	Inventory inventory;

	// Iterate through these to check for each key press
	List<KeyCode> keys;

	void OnLevelWasLoaded()
	{
		Init ();
	}

	void Start () {
		Init ();
	}

	// Use this for initialization (called by both Start and OnLevelWasLoaded
	void Init() {
		inventory = GameObject.Find ("Game Manager").GetComponent<Inventory> ();

		keys = new List<KeyCode> { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, 
			KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8
		};
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < keys.Count; i++) {
			if (Input.GetKeyDown (keys[i])) {
				if (inventory.items [i] != null) {
					Debug.Log (inventory.items [i].name + " " + inventory.items [i].checkType ());
					if (inventory.items [i].checkType ()) {
						inventory.items [i].useItem ();
						inventory.updateStack (inventory.items [i]);
					}
				}
			}
		}
	}
}
