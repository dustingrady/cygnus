using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "AcidFlatsQuestCheck", menuName = "Dialogue/Actions/AcidFlatsQuestCheck")]
public class AcidFlatsQuestCheck : DialogueAction {
	GameManager gm;

	private bool part1 = false;
	private bool part2 = false;
	private bool part3 = false;

	override public void Activate(GameObject npc) {
		gm = GameManager.instance;
		Inventory inv = gm.gameObject.GetComponent<Inventory> ();

		foreach (Item item in inv.items) {
			if (item != null) {
				if (item.name == "ProtocolOverride Part 1") {
					part1 = true;
				}

				if (item.name == "ProtocolOverride Part 2") {
					part2 = true;
				}

				if (item.name == "ProtocolOverride Part 3") {
					part3 = true;
				}
			}
		}

		if (part1 && part2 && part3) {
			gm.CompleteQuest (420);
		}
	}
}
