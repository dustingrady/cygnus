using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class TheEnd : MonoBehaviour {
	public Sprite standing;
	bool zoomingIn = false;
	CinemachineVirtualCamera cam;
	CinemachineConfiner confiner;

	void Start() {
		cam = GameObject.Find ("FollowCM").GetComponent<Cinemachine.CinemachineVirtualCamera> ();
		confiner = GameObject.Find ("FollowCM").GetComponent<Cinemachine.CinemachineConfiner> ();

		var partsCollected = CheckPartsCollected ();
		if (partsCollected != 4) {
			Destroy (this.gameObject);
		} else {
			GameManager.instance.backgroundMusicVolume = 0;
		}
	}

	// Returns the number of ship parts collected
	int CheckPartsCollected() {
		int[] questNums = {199, 299, 399, 499};
		int completed = 0;
		foreach (int quest in questNums) {
			if (GameManager.instance.CheckQuestComplete (quest)) {
				completed++;
			}
		}

		return completed;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.CompareTag ("Player") && GameManager.instance.CheckQuestComplete(999)) {
			Debug.Log ("trigger ending, grats!");
			EndTheGame ();
		}
	}

	void Update() {
		if (zoomingIn) {
			cam.m_Lens.OrthographicSize -= Time.deltaTime;

			if (cam.m_Lens.OrthographicSize <= 1f) {
				zoomingIn = false;
				GameObject.Find ("End Text").transform.localScale = new Vector3 (0.2f, 0.2f, 1f);
			}
		}
	}

	void EndTheGame() {
		var player = GameObject.FindGameObjectWithTag ("Player");
		player.GetComponent<PlayerController> ().enabled = false;
		player.GetComponent<PlayerAnimation> ().enabled = false;
		player.GetComponent<Animator> ().enabled = false;
		player.GetComponent<SpriteRenderer> ().sprite = standing;
		GameObject.Find ("Bounds").SetActive (false);
		zoomingIn = true;
	}
}
