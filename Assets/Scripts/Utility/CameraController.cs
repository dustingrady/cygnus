using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour {
	CinemachineVirtualCamera vc;
	GameObject cameraAnchor;
	GameObject plr;

	void Start() {
		cameraAnchor = GameObject.Find ("Camera Target");
		plr = GameObject.FindGameObjectWithTag ("Player");
		vc = gameObject.GetComponent<CinemachineVirtualCamera> ();
	}

	void Update() {
		
	}

	public void FocusForTime(GameObject target, float time) {
		StartCoroutine(MoveToTarget(target, time));
	}

	IEnumerator MoveToTarget(GameObject target, float time) {
		cameraAnchor.transform.position = target.transform.position;
		plr.GetComponent<PlayerController> ().enabled = false;
		plr.GetComponent<PlayerAnimation> ().enabled = false;

		yield return new WaitForSeconds(time);

		cameraAnchor.transform.position = plr.transform.position;
		plr.GetComponent<PlayerController> ().enabled = true;
		plr.GetComponent<PlayerAnimation> ().enabled = true;
	}


}
