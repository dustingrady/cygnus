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
	}


	void Update() {
		if (movingToShip) {
			transform.position = new Vector3 (transform.position.x - Time.deltaTime * 4, transform.position.y, transform.position.z);
		}
	}


	void HappyBot(bool isHappy) {
		if (isHappy) {
			GetComponent<SpriteRenderer> ().sprite = happySpt;
		} else {
			GetComponent<SpriteRenderer> ().sprite = angrySpt;
		}
	}


	void StartMoveToShip(int id) {
		if (id == 1) {
			Debug.Log ("Moving to ship");
			//StartCoroutine("MoveToShip");
		}
	}

	public IEnumerator MoveToShip() {
		Debug.Log ("Moving the bot!");
		movingToShip = true;
		yield return new WaitForSeconds (5f);
		movingToShip = false;
	}
}
