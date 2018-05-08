using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	private AudioController ac;
	public GameObject miniBoss;
	bool active = false;

	// Use this for initialization
	void Start () {
		GameObject camera = GameObject.Find("Main Camera");
		ac = camera.GetComponent<AudioController>();
	}

	void Update(){
		if (miniBoss.activeInHierarchy) {
			active = true;
		}

		if (active && !miniBoss.activeInHierarchy) {
			ac.source.Stop ();
			ac.source.clip = ac.audio [0]; 
			ac.source.Play ();
			active = false;
		}
	}
}
