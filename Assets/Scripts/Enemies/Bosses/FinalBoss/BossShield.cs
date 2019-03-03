using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShield : MonoBehaviour {
	public GameObject parent;

	void Start() {
		if (GameManager.instance.CheckQuestComplete(999)) {
			Destroy(this.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		transform.position = parent.transform.position;
	}
}
