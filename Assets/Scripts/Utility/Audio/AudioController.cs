using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
	public AudioClip[] audio;
	public AudioSource source;

	void Start () {
		source = gameObject.GetComponent<AudioSource> ();
		source.clip = audio [0]; //First song in array will be background track
		source.Play ();
	}
	
	void Update () {
		
	}
}
