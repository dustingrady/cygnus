using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest")]
public class Quest : ScriptableObject {
	public string name;
	public int id;
	public bool completed = false;
}
