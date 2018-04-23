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
				if (item.name == "Protocol Override Part 1") {
					inv.updateStack (item);
				}

				if (item.name == "Protocol Override Part 2") {
					inv.updateStack (item);
				}

				if (item.name == "Protocol Override Part 3") {
					inv.updateStack (item);
				}
			}
		}
		inv.addItem (po);
	}
}
