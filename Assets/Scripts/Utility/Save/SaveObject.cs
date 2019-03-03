using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveObject {
	public SaveFile one;
	public SaveFile two;
	public SaveFile three;
}

[System.Serializable]
public class SaveFile {
	public string scene = "";
	public Vector3 playerPosition;
	public bool hasGloves = false;
	public List<int> questsComplete = new List<int>();
}