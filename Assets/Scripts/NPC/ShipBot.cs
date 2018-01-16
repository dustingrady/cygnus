using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipBot : MonoBehaviour {

	public Sprite angrySpt;
	public Sprite happySpt;

	public NPCTalk talker;
	public TalkTree outsideDialogue;
	public TalkTree gloveDialogue;
	public TalkTree inside_fireOut;

	bool movingToShip;

	GameManager gm;

	void Awake() {
		GameManager.QuestCompleted += StartMoveToShip;
	}

	// Use this for initialization
	void Start () {
		gm = GameManager.instance;
		talker = GetComponent<NPCTalk> ();

		// If the scene is the ship exterior and the fire quest is still not complete, be visible
		// and use the outside ship dialog
		if (SceneManager.GetActiveScene ().name == "ShipExterior") {
			gameObject.SetActive (true);

			if (gm.CheckQuestComplete (0) == false) {
				talker.tree = outsideDialogue;
			} else if (gm.CheckQuestComplete(1) == false) {
				talker.tree = gloveDialogue;
				transform.position = new Vector3 (6.5f, -8.5f, 0f);
			} else {
				gameObject.SetActive (false);
			}
		}

		// Ship Scene
		if (SceneManager.GetActiveScene ().name == "Ship") {
			if (gm.CheckQuestComplete (1) == true) {
				talker.tree = inside_fireOut;
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


	void HappyBot(bool isHappy) {
		if (isHappy) {
			GetComponent<SpriteRenderer> ().sprite = happySpt;
		} else {
			GetComponent<SpriteRenderer> ().sprite = angrySpt;
		}
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
}
