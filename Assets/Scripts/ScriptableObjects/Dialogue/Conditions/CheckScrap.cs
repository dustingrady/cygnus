using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ScrapCondition", menuName = "Dialogue/Conditions/ScrapCondition")]

public class CheckScrap : DialogueCondition {
	public int requiredScrap;

	override public bool Check(GameObject npc) {
		Inventory inv = GameManager.instance.GetComponent<Inventory> ();

		if (inv.currentScrap >= requiredScrap) {
			return true;
		} else {
			return false;
		}
	}
}
