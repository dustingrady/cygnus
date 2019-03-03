using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public struct QuestInfo {
    public string name;
    public bool complete;

    public QuestInfo(string n, bool comp) {
        name = n;
        complete = comp;
    }
}

public struct Position {
    public float x, y, z;
    
    public Position(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    public static implicit operator Position(Vector3 rvalue) {
        return new Position(rvalue.x, rvalue.y, rvalue.z);
    }
}

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	private const int targetFramerate = 60;

	[Header("User Settings")]
	public float backgroundMusicVolume = 0.3f;

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
    public SaveSlot slot;


	void Start() {
		// Set the target framerate
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = targetFramerate;

		if (loadGame)
		{
			SaveMan.Load(slot);
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
	}

	// ------
	// Save, Load, and Checkpoints
	// ------

    public void OnLoadGame(Dictionary<SaveType, object> dict, SaveSlot s) {
        if (!loadGame) {
            var scene = (string)dict[SaveType.SCENE];
            hasGloves = (bool)dict[SaveType.GLOVES];
            Debug.Log(scene);
            SceneManager.LoadScene(scene);
            slot = s;
            loadGame = true;
        }
        else {
            quests.Clear();
            var quest_info = (List<QuestInfo>)dict[SaveType.QUESTS];
            foreach(var quest in quest_info) {
                var inst = (Quest)Instantiate(Resources.Load("ScriptableObjects/Quests/" + quest.name));
                inst.completed = quest.complete;
                quests.Add(inst);
            }
        }
    }

    public void OnSaveGame(Dictionary<SaveType, object> dict, SaveSlot s) {
        dict.Add(SaveType.SCENE, SceneManager.GetActiveScene().name);
        dict.Add(SaveType.GLOVES, hasGloves); 
        var save_quests = new List<QuestInfo>();

        foreach(var quest in quests) {
            var quest_name = ((ScriptableObject)quest).name;
            quest_name = quest_name.Substring(0, quest_name.IndexOf('('));
            save_quests.Add(new QuestInfo(name, quest.completed));
        }
        dict.Add(SaveType.QUESTS, save_quests);
    }

	public void SaveGame(int slot) {
		Saver.SaveGame (slot);
	}

	public void LoadGame(int slot) {
		Saver.LoadGame (slot);
	}
		
	// ------
	// Quests
	// ------

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
