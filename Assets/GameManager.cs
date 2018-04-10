using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	private const int targetFramerate = 60;

	public int totalScrap = 0;
	public int currentScrap = 0;

	public List<Quest> quests;
	public List<Quest> questsBase;
	public delegate void QuestEvent(int questId);
	public static event QuestEvent QuestCompleted;

	public GameObject player;
	public bool controllerConnected = false;

	public bool hasGloves = false;
	bool loadGame = false;

	public string previousLocation;

    public void OnLoadGame(Dictionary<SaveType, object> dict) {
        if (!loadGame) {
            var scene = (string)dict[SaveType.SCENE];
            Debug.Log(scene);
            SceneManager.LoadScene(scene);
            loadGame = true;
        }
    }

    public void OnSaveGame(Dictionary<SaveType, object> dict) {
        dict.Add(SaveType.SCENE, SceneManager.GetActiveScene().name);
    }

    void Start() {
		// Set the target framerate
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = targetFramerate;

        if (loadGame)
        {
            SaveMan.Load();
            loadGame = false;
        }
	}

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

		// Fill questBase from Resources Folder
		Object[] loadedQuests = Resources.LoadAll("ScriptableObjects/Quests", typeof(Quest));

		foreach(Object quest in loadedQuests) {
			questsBase.Add ((Quest)quest);
		}

		// Fill the quests from the questBase
		foreach (Quest quest in questsBase) {
			Quest q = Instantiate (quest);
			quests.Add (q);
		}

        SaveMan.SaveGame += OnSaveGame;
        SaveMan.LoadGame += OnLoadGame;
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
