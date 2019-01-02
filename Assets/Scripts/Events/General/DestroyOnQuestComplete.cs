using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnQuestComplete : MonoBehaviour {
	public int questNumber;
	void Awake() {
		if (GameManager.instance.CheckQuestComplete (questNumber)) {
			Destroy (this.gameObject);
		}
	}
}
