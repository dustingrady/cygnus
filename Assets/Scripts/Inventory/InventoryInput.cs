using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInput : MonoBehaviour {

	Inventory inventory;
	Player player;
	ElementManager EM;
	ElementUI EU;

	void OnLevelWasLoaded()
	{
		inventory = GameObject.Find ("Game Manager").GetComponent<Inventory> ();
		player = GameObject.Find ("Player").GetComponent<Player> ();
		EM = GameObject.Find ("Elements").GetComponent<ElementManager> ();
		EU = GameObject.Find ("UI").GetComponent<ElementUI> ();
	}
	// Use this for initialization
	void Start () {
		inventory = GameObject.Find ("Game Manager").GetComponent<Inventory> ();
		player = GameObject.Find ("Player").GetComponent<Player> ();
		EM = GameObject.Find ("Elements").GetComponent<ElementManager> ();
		EU = GameObject.Find ("UI").GetComponent<ElementUI> ();
		Debug.Log (EM + " " + EU);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			if (inventory.items [0] != null) {
				Debug.Log (inventory.items [0].name + " " + inventory.items [0].checkType ());
				if (inventory.items [0].checkType ()) {
					float heal = inventory.items [0].useConsumable ();
					string element = inventory.items [0].consumeCombo ();
					if (heal != 0) {
						if (player.health.CurrentVal < player.health.MaxVal) {
							player.health.CurrentVal += heal;
							inventory.updateStack (inventory.items [0]);
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
						EU.UpdateElements ();
					}
				}
			}
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			if (inventory.items [1] != null) {
				Debug.Log (inventory.items [1].name + " " + inventory.items [1].checkType ());
				if (inventory.items [1].checkType ()) { 
					float heal = inventory.items [1].useConsumable ();
					string element = inventory.items [0].consumeCombo ();
					if (heal != 0) {
						if (player.health.CurrentVal < player.health.MaxVal) {
							player.health.CurrentVal += inventory.items [1].useConsumable ();
							inventory.updateStack (inventory.items [1]);
						}
					}
				}
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			if(inventory.items [2] != null)
			{
				Debug.Log (inventory.items [2].name + " " + inventory.items [2].checkType());
				if (inventory.items [2].checkType()){
					float heal = inventory.items [2].useConsumable ();
					string element = inventory.items [0].consumeCombo ();
					if (heal != 0) {
						if (player.health.CurrentVal < player.health.MaxVal) {
							player.health.CurrentVal += inventory.items [2].useConsumable ();
							inventory.updateStack (inventory.items [2]);
						}
					}
				}
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			if(inventory.items [3] != null)
			{
				Debug.Log (inventory.items [3].name + " " + inventory.items [3].checkType());
				if (inventory.items [3].checkType()) {
					float heal = inventory.items [3].useConsumable ();
					string element = inventory.items [0].consumeCombo ();
					if (heal != 0) {
						if (player.health.CurrentVal < player.health.MaxVal) {
							player.health.CurrentVal += inventory.items [3].useConsumable ();
							inventory.updateStack (inventory.items [3]);
						}
					}
				}
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha5))
		{
			if(inventory.items [4] != null)
			{
				Debug.Log (inventory.items [4].name + " " + inventory.items [4].checkType());
				if (inventory.items [4].checkType()) {
					float heal = inventory.items [4].useConsumable ();
					string element = inventory.items [0].consumeCombo ();
					if (heal != 0) {
						if (player.health.CurrentVal < player.health.MaxVal) {
							player.health.CurrentVal += inventory.items [4].useConsumable ();
							inventory.updateStack (inventory.items [4]);
						}
					}
				}
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha6))
		{
			if(inventory.items [5] != null)
			{
				Debug.Log (inventory.items [5].name + " " + inventory.items [5].checkType());
				if (inventory.items [5].checkType()) {
					float heal = inventory.items [5].useConsumable ();
					string element = inventory.items [0].consumeCombo ();
					if (heal != 0) {
						if (player.health.CurrentVal < player.health.MaxVal) {
							player.health.CurrentVal += inventory.items [5].useConsumable ();
							inventory.updateStack (inventory.items [5]);
						}
					}
				}
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha7))
		{
			if(inventory.items [6] != null)
			{
				Debug.Log (inventory.items [6].name + " " + inventory.items [6].checkType());
				if (inventory.items [6].checkType()) {
					float heal = inventory.items [6].useConsumable ();
					string element = inventory.items [0].consumeCombo ();
					if (heal != 0) {
						if (player.health.CurrentVal < player.health.MaxVal) {
							player.health.CurrentVal += inventory.items [6].useConsumable ();
							inventory.updateStack (inventory.items [6]);
						}
					}
				}
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha8))
		{
			if(inventory.items [7] != null)
			{
				Debug.Log (inventory.items [7].name + " " + inventory.items [7].checkType());
				if (inventory.items [7].checkType()) {
					float heal = inventory.items [7].useConsumable ();
					string element = inventory.items [0].consumeCombo ();
					if (heal != 0) {
						if (player.health.CurrentVal < player.health.MaxVal) {
							player.health.CurrentVal += inventory.items [7].useConsumable ();
							inventory.updateStack (inventory.items [7]);
						}
					}
				}
			}
		}
	}
}
