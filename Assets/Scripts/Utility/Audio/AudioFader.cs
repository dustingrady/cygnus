using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFader : MonoBehaviour {

	public float startVolume;
	public float endVolume;
	public float fadeTime;
	public bool useBackgroundVolumeAsTarget;
	public AudioSource audio;

	// Use this for initialization
	void Start () {
		if (useBackgroundVolumeAsTarget) {
			endVolume = GameManager.instance.backgroundMusicVolume;
			console.log (endVolume);
		}
		audio = GameManager.instance.GetComponent<AudioSource> ();
		audio.volume = startVolume;
	}

	void Update() {
		var targetVolume = useBackgroundVolumeAsTarget ? GameManager.instance.backgroundMusicVolume : endVolume;
		if (audio.volume < targetVolume) {
			audio.volume += (Time.deltaTime * endVolume) / fadeTime;
		}
	}
}
