using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "QuestCondition", menuName = "Dialogue/Conditions/QuestCondition")]

public class CheckQuest : DialogueCondition {
	public int questId;

	override public bool Check(GameObject npc) {
		if (GameManager.instance.CheckQuestComplete(questId)) {
			Debug.Log ("the quest is complete");
			return true;
		} else {
			return false;
		}
	}
}
