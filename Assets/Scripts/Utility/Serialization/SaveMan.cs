using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public enum SaveType
{
	PLAYER,
	SCENE,
	INVENTORY,
	QUESTS
}

public delegate void SaveHandler(Dictionary<SaveType, object> dict);

public class SaveMan : MonoBehaviour {
    static string savePath;
    static GameManager gameManager;

	public static event SaveHandler SaveGame;
	public static event SaveHandler LoadGame;

    void Start()
    {
        savePath = Application.persistentDataPath + "/save.dat";
        gameManager = GameManager.instance;
        if (gameManager.loadGame)
        {
            Load();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Load();
        }
    }

	static void Serialize(Dictionary<SaveType, object> dict) {
		var binFormat = new BinaryFormatter();
		var fstream = new FileStream(savePath, FileMode.Create);
		binFormat.Serialize(fstream, dict);
		fstream.Close();
	}

	static Dictionary<SaveType, object> Deserialize() {
		var binFormat = new BinaryFormatter();
		var fstream = new FileStream(savePath, FileMode.OpenOrCreate);
		var dict = (Dictionary<SaveType, object>)binFormat.Deserialize(fstream);
		fstream.Close();

		return dict;
	}

    public static void Save()
    {
		var dict = new Dictionary<SaveType, object>();
		SaveGame(dict);	
		Serialize(dict);
        Debug.Log("Game Saved");
    }

    public static void Load()
    {
		var dict = Deserialize();
		LoadGame(dict);
        Debug.Log("Game Loaded");
    }
}
