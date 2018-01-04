using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCombo {
	public string[] keys;
	public int currentIndex = 0;

	public float lastKeyTime;
	public float timeout = 0.5f;

	public bool keysReleased = true;

	public KeyCombo(string[] keys) {
		this.keys = keys;
	}

	public bool Check() {
		if (Time.time > lastKeyTime + timeout) {
			currentIndex = 0;
		}

		if (currentIndex < keys.Length) {
			if (Input.GetButton (keys [currentIndex])) {
				currentIndex++;
				lastKeyTime = Time.time;
			}
		}

		if (currentIndex >= keys.Length) {
			currentIndex = 0;
			return true;
		} else {
			return false;
		}
	}

	public bool CheckDown() {
		if (Time.time > lastKeyTime + timeout) {
			currentIndex = 0;
		}

		if (currentIndex < keys.Length) {
			if (Input.GetButton (keys [currentIndex])) {
				currentIndex++;
				lastKeyTime = Time.time;
			}
		}

		if (Input.GetButton (keys [0]) == false) {
			keysReleased = true;
		}

		if (currentIndex >= keys.Length && keysReleased) {
			currentIndex = 0;
			keysReleased = false;
			return true;
		} else {
			return false;
		}
	}


}
