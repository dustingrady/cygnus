using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
	public AudioClip[] audio;
	public AudioSource source;
	public float startingVolume;

	void Start () {
		source = gameObject.GetComponent<AudioSource> ();

		if (audio.Length > 0) {
			source.volume = startingVolume;
			source.clip = audio [0]; //First song in array will be main background track
			source.Play ();
		}
	}

	public void ChangeVolume(float val) {
		GameManager.instance.backgroundMusicVolume = val;
		source.volume = val;
	}
	
	void Update () {
		
	}
}
