using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalk : MonoBehaviour
{

    public GameObject uiRoot;
    public TalkTree tree;
    public Image portrait;
    public GameObject windowPrefeb;

    public void StartDialogue()
    {
        GameObject win = Instantiate(windowPrefeb, uiRoot.transform);
        NPCDialogue d = win.GetComponent<NPCDialogue>();

        tree.Reset();
        d.dialogue = tree;
        d.potrait = portrait;
        d.UpdateDialogue();
    }
}
