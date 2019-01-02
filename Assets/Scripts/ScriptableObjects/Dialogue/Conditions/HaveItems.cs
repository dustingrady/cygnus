using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "HaveItemsCondition", menuName = "Dialogue/Conditions/HaveItemsCondition")]

public class HaveItems : DialogueCondition {
	public string[] requiredItems;

	override public bool Check(GameObject npc) {

		// Gets a reference to the inventory through the instance Singleton
		Inventory inv = GameManager.instance.GetComponent<Inventory> ();

		foreach (var item in requiredItems) {
			Debug.Log(item);	
		}

		int total = 0;
		if (inv.items != null) {
			foreach (var item in inv.items) {
				if (item != null) {
					if (System.Array.IndexOf (requiredItems, item.itemName) != -1) {
						total += 1;
					} 
				}
			}
		}

		if (total == requiredItems.Length) {
			return true;
		} else {
			return false;
		}
	}
}