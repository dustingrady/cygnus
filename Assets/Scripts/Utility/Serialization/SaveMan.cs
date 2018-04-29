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

public enum SaveSlot : string
{
    QUICK = "Q",
    ONE = "1",
    TWO = "2",
    THREE = "3"
}

public delegate void SaveHandler(Dictionary<SaveType, object> dict);

public class SaveMan : MonoBehaviour {
    static string savePath;
    static GameManager gameManager;

	public static event SaveHandler SaveGame;
	public static event SaveHandler LoadGame;

    void Start()
    {
        savePath = Application.persistentDataPath + "/save";
        gameManager = GameManager.instance;
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

	static void Serialize(Dictionary<SaveType, object> dict, SaveSlot slot) {
		var binFormat = new BinaryFormatter();
		var fstream = new FileStream(savePath + slot + ".dat", FileMode.Create);
		binFormat.Serialize(fstream, dict);
		fstream.Close();
	}

	static Dictionary<SaveType, object> Deserialize(SaveSlot slot) {
		var binFormat = new BinaryFormatter();
		var fstream = new FileStream(savePath + slot + ".dat", FileMode.OpenOrCreate);
		var dict = (Dictionary<SaveType, object>)binFormat.Deserialize(fstream);
		fstream.Close();

		return dict;
	}

    public static void Save(SaveSlot slot = SaveSlot.QUICK)
    {
		var dict = new Dictionary<SaveType, object>();
		SaveGame(dict);	
		Serialize(dict, slot);
        Debug.Log("Game Saved");
    }

    public static void Load(SaveSlot slot = SaveSlot.QUICK)
    {
		var dict = Deserialize(slot);
		LoadGame(dict);
        Debug.Log("Game Loaded");
    }
}
