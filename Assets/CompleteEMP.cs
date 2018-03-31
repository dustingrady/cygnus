using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteEMP : MonoBehaviour
{

    public NPCTalk talker;
    public TalkTree giveQuest_EMP;
    public TalkTree killBoss;

    bool batteryInHand;
	bool dempInHand;

    GameManager gm;
    Inventory inventory;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
	}
		
    void Update()
	{
		gm = GameManager.instance;

		talker = GetComponent<NPCTalk>();
		inventory = gm.gameObject.GetComponent<Inventory>();
		/*foreach (Item item in inventory) {
			if (item.name == "EMP Part 1") {
				batteryInHand = true;
				Debug.Log ("This worked");
			}

			if (item.name == "EMP Part 2") {
				dempInHand = true;
				Debug.Log ("This worked");
		}
    }
    */
    }
}