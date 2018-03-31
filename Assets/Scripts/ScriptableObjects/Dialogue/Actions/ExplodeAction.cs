using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ExplodeAction", menuName = "Dialogue/Actions/ExplodeAction")]
public class ExplodeAction : DialogueAction {
	public GameObject explodePrefab;

	override public void Activate(GameObject npc) {
		GameObject expld = Instantiate(explodePrefab, npc.transform);
		expld.transform.parent = null;
		Destroy(npc);
	}
}