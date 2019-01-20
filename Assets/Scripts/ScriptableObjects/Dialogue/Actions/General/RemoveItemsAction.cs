using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "RemoveItemsAction", menuName = "Dialogue/Actions/RemoveItemsAction")]
public class RemoveItemsAction : DialogueAction {
	public string[] itemsToRemove;
	override public void Activate(GameObject npc) {
		GameManager gm = GameManager.instance;
		Inventory inv = gm.gameObject.GetComponent<Inventory>();
		foreach (Item item in inv.items) {
			if (item != null) {
				foreach (string itemToRemove in itemsToRemove) {
					if (item.name == itemToRemove) {
						Debug.Log ("Removing " + itemToRemove + " from invenotry");
						inv.updateStack (item);
					}
				}
			}
		}
	}
}
