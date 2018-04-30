using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ProtocolAction", menuName = "Dialogue/Actions/ProtocolAction")]
public class ProtocolAction : DialogueAction {

	GameManager gm;
	public Item po;

	override public void Activate(GameObject npc) {
		gm = GameManager.instance;

		Inventory inv = gm.gameObject.GetComponent<Inventory>();
		foreach (Item item in inv.items) {
			if (item != null) {
				if (item.name == "ProtocolOverride Part 1") {
					inv.removeItem (item);
				}

				if (item.name == "ProtocolOverride Part 2") {
					inv.removeItem (item);
				}

				if (item.name == "ProtocolOverride Part 3") {
					inv.removeItem (item);
				}
			}
		}
		inv.addItem (po);
	}
}
