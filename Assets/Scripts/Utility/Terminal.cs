using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Terminal : MonoBehaviour {

	public GameObject textLine;
	public List<string> messages;
	public GameObject anchor;
	public int messageIndex = 0;

	// Use this for initialization
	void Start () {
		GameObject msg = Instantiate (textLine, anchor.transform);
		msg.GetComponent<Typewriter> ().SetText (messages [messageIndex]);
		messageIndex++;
	}

	// Update is called once per frame
	public void AddMessage () {
		if (messageIndex < messages.Count) {
			GameObject msg = Instantiate (textLine, anchor.transform);
			msg.GetComponent<Typewriter> ().SetText (messages [messageIndex]);
			messageIndex++;
		} else {
			Debug.Log("starting game");
			StartGame ();
		}
	}

	public void StartGame() {
		SceneManager.LoadScene ("Ship");
	}

}
