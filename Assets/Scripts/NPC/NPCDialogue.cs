using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour {

    public TalkTree dialogue;
    public Text npcText;
    public Text[] choices;
    public Image potrait;
    public delegate void DialogueEnd();
    public DialogueEnd signal;

    public void UpdateDialogue()
    {
        Debug.Log("Dialogue State: " + dialogue.CurrentState);
        TalkState s = dialogue.State;

        if (s.endTree)
        {
            signal();
            Destroy(gameObject);
        }
        npcText.text = s.text;
        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].text = s.choices[i].response;
        }
    }

    public void OnDialogueChoice(int c)
    {
        dialogue.Advance(c);
        UpdateDialogue();
    }
}
