using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ChangeScrapAction", menuName = "Dialogue/Actions/ChangeScrapAction")]
public class ChangeScrapAction : DialogueAction {
	public int changeAmount;

	override public void Activate(GameObject npc) {
		Inventory inv = GameManager.instance.GetComponent<Inventory> ();
		inv.addScrap (changeAmount);
	}
}
