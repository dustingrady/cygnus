using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipBot : MonoBehaviour {

	public NPCTalk talker;

	[Header("Talk Trees")]
	public TalkTree outsideDialogue;
	public TalkTree gloveDialogue;
	public TalkTree inside_fireOut;
	public TalkTree collected_one;
	public TalkTree collected_two;
	public TalkTree collected_three;
	public TalkTree collected_four;


	bool movingToShip;

	GameManager gm;

	void Awake() {
		GameManager.QuestCompleted += StartMoveToShip;
	}

	// Use this for initialization
	void Start () {

		gm = GameManager.instance;
		talker = GetComponent<NPCTalk> ();

		// Check how many parts have been collected
		int collectedParts = CheckPartsCollected();

		// If the scene is the ship exterior and the fire quest is still not complete, be visible
		// and use the outside ship dialog
		if (SceneManager.GetActiveScene ().name == "ShipExterior") {
			gameObject.SetActive (true);

			if (gm.CheckQuestComplete (0) == false) {
				talker.tree = outsideDialogue;
			} else if (gm.CheckQuestComplete (1) == false) {
				talker.tree = gloveDialogue;
				transform.position = new Vector3 (6.5f, -8.5f, 0f);
			} else {
				gameObject.SetActive (false);
			}
		}

		// Ship Scene
		if (SceneManager.GetActiveScene ().name == "Ship") {
			if (gm.CheckQuestComplete (1) == true) {
				if (collectedParts == 0) {
					talker.tree = inside_fireOut;
				} else if (collectedParts == 1) {
					talker.tree = collected_one;
				} else if (collectedParts == 2) {
					talker.tree = collected_two;
				} else if (collectedParts == 3) {
					talker.tree = collected_three;
				} else if (collectedParts == 4) {
					talker.tree = collected_four;
				}
			}
		}
	}


	void Update() {

		// When fire is put out, move to ship
		if (movingToShip) {
			if (transform.position.x > -16) {
				// Move towards the ship
				transform.position = new Vector3 (transform.position.x - Time.deltaTime * 4, transform.position.y, transform.position.z);
			} else {
				Destroy (this.gameObject);
			}
		}
	}


	void OnDestroy() {
		// Removing shipbot reference when the scene is changed
		GameManager.QuestCompleted -= StartMoveToShip;
	}


	// Used when the fire is put out
	void StartMoveToShip(int id) {
		if (id == 1) {
			Debug.Log ("Moving to ship");
			// Disable trigger
			GetComponent<BoxCollider2D>().enabled = false;
			movingToShip = true;
		}
	}


	// Returns the number of ship parts collected
	int CheckPartsCollected() {
		int[] questNums = {199, 299, 399, 499 };
		int completed = 0;
		foreach (int quest in questNums) {
			if (gm.CheckQuestComplete (quest)) {
				completed++;
			}
		}
		Debug.Log (completed);
		return completed;
	}
}
