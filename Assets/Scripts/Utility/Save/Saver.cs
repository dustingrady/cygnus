using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public static class Saver {
	public static bool loading;
	public static Vector3 position;

	public static void SaveGame(int slot) {
		string directory = "/saves/";
		string fileName = "";
		if (slot == 1) {
			fileName = "one.json";
		} else if (slot == 2) {
			fileName = "two.json";
		} else if (slot == 3) {
			fileName = "three.json";
		} else {
			fileName = "one.json"; // default to file one
		}

		// check quests
		List<int> completedQuests = new List<int>();
		foreach (Quest quest in GameManager.instance.quests) {
			if (quest.completed) {
				completedQuests.Add (quest.id);
			}
		}

		// get player position
		Debug.Log(GameManager.instance);
		Vector3 playerPos = GameManager.instance.player.transform.position;

		SaveFile saveState = new SaveFile {
			scene = SceneManager.GetActiveScene().name,
			hasGloves = true,
			playerPosition = playerPos,
			questsComplete = completedQuests,
		};

		string json = JsonUtility.ToJson (saveState);

		Debug.Log (Application.dataPath + directory);
		Directory.CreateDirectory (Application.dataPath + directory);
		File.WriteAllText (Application.dataPath + directory + fileName, json);
	}

	public static void LoadGame(int slot) {
		SaveFile loadedFile = LoadSaveFile (slot);

		if (loadedFile != null) {
			Debug.Log (loadedFile.scene);

			foreach (int questId in loadedFile.questsComplete) {
				Debug.Log ("Completed quest: " + questId);
				GameManager.instance.CompleteQuest (questId);
			}

			loading = true;
			GameManager.instance.hasGloves = loadedFile.hasGloves;
			position = loadedFile.playerPosition;
			SceneManager.LoadScene (loadedFile.scene);
		}
	}

	public static SaveFile LoadSaveFile(int slot) {
		string directory = "/saves/";
		string fileName = "";
		if (slot == 1) {
			fileName = "one.json";
		} else if (slot == 2) {
			fileName = "two.json";
		} else if (slot == 3) {
			fileName = "three.json";
		} else {
			console.log ("invalid save file");
			return null;
		}

		if (File.Exists (Application.dataPath + directory + fileName)) {
			console.log (Application.dataPath + directory + fileName);
			string saveString = File.ReadAllText (Application.dataPath + directory + fileName);
			SaveFile loadedFile = JsonUtility.FromJson<SaveFile> (saveString);
			console.log (loadedFile);
			return loadedFile;
		} else {
			return null;
		}
	}
}
