using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueCondition : ScriptableObject {
	public abstract bool Check (GameObject npc);
}
