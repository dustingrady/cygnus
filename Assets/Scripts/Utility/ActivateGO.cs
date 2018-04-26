using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGO : MonoBehaviour {

	public GameObject go;

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.name == "Player") {
			if (go != null && !go.activeInHierarchy) {
				go.SetActive (true);
			}
		}
	}
}
