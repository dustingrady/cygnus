using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public int totalScrap = 0;
	public int currentScrap = 0;

	public List<Quest> quests;
	public List<Quest> questsBase;
	public delegate void QuestEvent(int questId);
	public static event QuestEvent QuestCompleted;

	public GameObject player;
	public bool controllerConnected = false;

	public bool hasGloves = false;

	public string previousLocation;

	void Awake () {
		// Allow the game manage to survive scene transition
		DontDestroyOnLoad (transform.gameObject);

		// Disable cursor
		Cursor.visible = false;

		// Creates singleton game manager
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		// Fill the quests from the questBase
		foreach (Quest quest in questsBase) {
			Quest q = Instantiate (quest);
			quests.Add (q);
		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckControllerStatus ();
	}


	void CheckControllerStatus() {
		string[] joystickNames = Input.GetJoystickNames ();

		if (joystickNames.Length > 0) {
			foreach (var str in joystickNames) {
				if (!string.IsNullOrEmpty (str) && controllerConnected == false) {
					controllerConnected = true;
				} else if (string.IsNullOrEmpty (str) && controllerConnected == true) {
					controllerConnected = false;
				}
			}
		}
	}


	public bool CheckQuestComplete(int id) {
		Quest q = quests.Find (x => x.id == id);

		if (q == null) {
			Debug.LogError("Quest " + id + " not found!"); 
		} 

		return q.completed;
	}

	public void CompleteQuest(int id) {
		Quest q = quests.Find (x => x.id == id);

		if (q == null) {
			Debug.LogError("Quest " + id + " not found!"); 
		}

		// Set the quest to complete
		q.completed = true;

		// Broadcast the quest complete
		if (QuestCompleted != null) {
			QuestCompleted (id);
		}
	}
}
