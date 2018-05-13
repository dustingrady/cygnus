using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
	public AudioClip[] audio;
	public AudioSource source;

	void Start () {
		source = gameObject.GetComponent<AudioSource> ();

		if (audio.Length > 0) {
			source.volume = GameManager.instance.backgroundMusicVolume;
			source.clip = audio [0]; //First song in array will be main background track
			source.Play ();
		}
	}
	
	void Update () {
		
	}
}
