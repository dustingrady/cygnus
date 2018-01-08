using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCTalk : MonoBehaviour
{

    public GameObject uiRoot;
    public TalkTree tree;
    public Sprite portrait;
    public GameObject windowPrefeb;
    bool inDialogue = false;

    public void EndDialogue()
    {
        inDialogue = false;
    }

    public void StartDialogue()
    {
        if (!inDialogue)
        {
            inDialogue = true;
            GameObject win = Instantiate(windowPrefeb, uiRoot.transform);
            NPCDialogue d = win.GetComponent<NPCDialogue>();

            tree.Reset();
            d.signal = EndDialogue;
            d.dialogue = tree;
            d.potrait.sprite = portrait;
            d.UpdateDialogue();
        }
    }
}
