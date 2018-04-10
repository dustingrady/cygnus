using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteEMP : MonoBehaviour
{
	public TalkTree giveQuest_CombineEMP;
    public TalkTree giveQuest_EMP;
    public TalkTree killBoss;

    bool batteryInHand;
	bool dEMPInHand;

    GameManager gm;
    Inventory inventory;

    void Awake(){
    }

    // Use this for initialization
    void Start(){
		gm = GameManager.instance;
		inventory = gm.gameObject.GetComponent<Inventory>();
	}
		
    void Update(){
		if (questComplete()) {
			NPCTalk talk = GetComponent<NPCTalk> ();
			talk.tree = giveQuest_CombineEMP;
		}
	}

	private bool questComplete(){
		//Debug.Log (inventory.items.Length);
		foreach (Item item in inventory.items) {
			if (item != null) {
				if (item.name == "EMP Part 1") {
					batteryInHand = true;
				}
				if (item.name == "EMP Part 2") {
					dEMPInHand = true;
				}
			}
		}
		if (batteryInHand && dEMPInHand) {
			return true;
		}
		return false;
	}

	private void destroyQuestItems() {
		foreach (Item item in inventory.items) {
			if (item != null) {
				if (item.name == "EMP Part 1") {
					inventory.updateStack(item);
				}
				if (item.name == "EMP Part 2") {
					inventory.updateStack(item);
				}
			}
		}
	}



}