using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "EMPAction", menuName = "Dialogue/Actions/EMPAction")]
public class EMPAction : DialogueAction {

	GameManager gm;
	public Item EMP;

	override public void Activate(GameObject npc) {
		gm = GameManager.instance;

		Inventory inv = gm.gameObject.GetComponent<Inventory>();
		foreach (Item item in inv.items) {
			if (item != null) {
				if (item.name == "EMP Part 1") {
					inv.updateStack (item);
				}

				if (item.name == "EMP Part 2") {
					inv.updateStack (item);
				}
			}
		}
		inv.addItem (EMP);
		inv.stackItem (EMP);
		inv.stackItem (EMP);
		inv.stackItem (EMP);
		inv.stackItem (EMP);
	}
}