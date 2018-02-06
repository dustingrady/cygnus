using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
struct GameData
{
    public string scene_name;
    public float player_health;
    public float player_pos_x;
    public float player_pos_y;

    public void Serialize(string savePath, bool serialize = true)
    {
        BinaryFormatter binFormat = new BinaryFormatter();
        FileStream fileStream = new FileStream(savePath, serialize ? FileMode.Create : FileMode.Open);
        if (serialize)
            binFormat.Serialize(fileStream, this);
        else
            this = (GameData)binFormat.Deserialize(fileStream);
        fileStream.Close();
    }
}

public class LoadMan : MonoBehaviour {
    static string savePath;
    static GameManager gameManager;

    void Start()
    {
        savePath = Application.persistentDataPath + "/save.dat";
        gameManager = GameManager.instance;
        if (gameManager.loadGame)
        {
            LoadData();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Saving...");
            SaveData();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Loading...");
            TestLoad();
        }
    }

    public static void SaveData()
    {
        //Create save struct
        GameData data = new GameData();

        //Get prerequisites
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 pos = player.transform.position;

        //Store fields
        data.scene_name = SceneManager.GetActiveScene().name;
        data.player_health = player.GetComponent<Player>().health.CurrentVal;
        data.player_pos_x = pos.x;
        data.player_pos_y = pos.y;

        //Serialize
        data.Serialize(savePath);

        Debug.Log("Game Saved");
    }

    static void LoadData()
    {
        //Create save struct
        GameData data = new GameData();

        //Deserialize
        data.Serialize(savePath, false);

        //Get prerequisites
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 pos = player.transform.position;

        //Expand fields
        player.GetComponent<Player>().health.CurrentVal = data.player_health;
        player.transform.position = new Vector3(data.player_pos_x, data.player_pos_y, 0);

        Debug.Log("Game Loaded");
    }

    public static void TestLoad()
    {
        GameData data = new GameData();
        data.Serialize(savePath, false);
        gameManager.loadGame = true;
        SceneManager.LoadScene(data.scene_name);
    }
}