using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFader : MonoBehaviour {

	public float startVolume;
	public float endVolume;
	public float fadeTime;
	public AudioSource audio;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		audio.volume = startVolume;
	}

	void Update() {
		if (audio.volume < endVolume) {
			
			audio.volume += (Time.deltaTime * endVolume) / fadeTime;
		}
	}
}
