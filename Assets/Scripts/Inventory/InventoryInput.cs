using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInput : MonoBehaviour {

	Inventory inventory;
	Player player;
	ElementManager EM;
	ElementUI EU;

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
		player = GameObject.Find ("Player").GetComponent<Player> ();
		EM = GameObject.Find ("Elements").GetComponent<ElementManager> ();
		EU = GameObject.Find ("UI").GetComponent<ElementUI> ();
		Debug.Log (EM + " " + EU);

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
						float heal = inventory.items [i].useConsumable ();
						string element = inventory.items [i].consumeCombo ();
						if (heal != 0) {
							if (player.health.CurrentVal < player.health.MaxVal) {
								player.health.CurrentVal += heal;
								inventory.updateStack (inventory.items [i]);
							}
						}

						if (element != "") {
							if (element == "lava") {
								EM.AssignToHand ("left", "FireElement");
								EM.AssignToHand ("right", "EarthElement");
							}
							if (element == "steam") {
								EM.AssignToHand ("left", "WaterElement");
								EM.AssignToHand ("right", "FireElement");
							}
							if (element == "magnet") {
								EM.AssignToHand ("left", "MetalElement");
								EM.AssignToHand ("right", "ElectricElement");
							}

							inventory.updateStack (inventory.items [i]);

							EU.UpdateElements ();
						}
					}
				}
			}
		}
	}
}
