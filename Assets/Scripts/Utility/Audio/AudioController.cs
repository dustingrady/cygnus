using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
	public AudioClip[] audio;
	public AudioSource source;
	public float startingVolume;

	void Start () {
		source = GameManager.instance.GetComponent<AudioSource> ();
		if (audio.Length > 0) {
			console.log (audio [0].name);
			if (audio.Length > 0 && source.clip != audio [0]) {
				source.clip = audio [0]; //First song in array will be main background track
				source.Play ();
			}
		}
	}

	public void ChangeVolume(float val) {
		console.log (val);
		GameManager.instance.backgroundMusicVolume = val;
		source.volume = val;
	}
}
