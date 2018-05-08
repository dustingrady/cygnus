/* 
 * Switches music for various encounters 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSwitch : MonoBehaviour {
	private AudioController ac;

	// Use this for initialization
	void Start () {
		GameObject camera = GameObject.Find("Main Camera");
		ac = camera.GetComponent<AudioController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			if (ac.audio [1] != null) {
				ac.source.Stop ();
				ac.source.clip = ac.audio [1]; //Switch to boss music
				ac.source.Play ();
				Destroy (this);
			}
		}

		/*
		if(){ //If we hit some other thing, play some other stuff

		}
		*/
	}
}
