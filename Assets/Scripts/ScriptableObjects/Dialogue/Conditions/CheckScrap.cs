using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ScrapCondition", menuName = "Dialogue/Conditions/ScrapCondition")]

public class CheckScrap : DialogueCondition {
	public int requiredScrap;

	override public bool Check(GameObject npc) {

		// Gets a reference to the inventory through the instance Singleton
		Inventory inv = GameManager.instance.GetComponent<Inventory> ();

		// This is the condition that should determine whether the dialog option is visible to the player
		// Different instances of this "CheckScrap" scriptable object can have different values for 'requiredScrap'
		if (inv.currentScrap >= requiredScrap) {
			return true;
		} else {
			return false;
		}
	}
}
