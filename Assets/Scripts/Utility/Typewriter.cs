using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour {
	public string buffer;
	public float timePerLetter;

	private Text textLabel;
	private string text;
	float elapsedTime = 0;
	int currentLetter = 0;
	public AudioClip kbSound;
	private bool initialRaiseComplete = false;

	public delegate void EnteredMsg(int msgLength);
	public static event EnteredMsg OnSubmit;

	void Start() {
		kbSound = Resources.Load ("Sounds/SFX/keyboard") as AudioClip;

		textLabel = GetComponent<Text> ();
		Typewriter.OnSubmit += RaiseText;
	}

	// Update is called once per frame
	void Update () {
		if (elapsedTime > timePerLetter && currentLetter < buffer.Length) {

			// Add the letter from the buffer;
			text = text + buffer[currentLetter];

			// Update the text on the screen
			textLabel.text = text;

			AudioSource.PlayClipAtPoint (kbSound, Camera.main.transform.position, volume:0.1f);

			currentLetter++;
			elapsedTime = 0;

			if (currentLetter == buffer.Length - 1) {
				StartCoroutine (Submit());
			}
		}

		elapsedTime += Time.deltaTime;
	}

	public void SetText(string msg) {
		buffer = msg;
		Debug.Log (msg.Length);
	}

	IEnumerator Submit() {
		yield return new WaitForSeconds (1f);
		OnSubmit (buffer.Length);

		transform.root.GetComponent<Terminal> ().AddMessage ();
	}

	void RaiseText(int msgLength) {
		if (msgLength > 85) {
			transform.position = new Vector3 (transform.position.x, transform.position.y + 70, transform.position.z);
		} else {
			transform.position = new Vector3 (transform.position.x, transform.position.y + 40, transform.position.z);
		}
	}


}
