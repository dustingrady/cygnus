using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActivation : MonoBehaviour {

	public GameObject target;
	public GameObject exit;
	bool showExit = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!target.activeInHierarchy) {
			showExit = true;
		}

		if (showExit) {
			exit.SetActive (true);
		}
	}
}
