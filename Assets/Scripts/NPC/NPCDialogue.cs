using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NPCDialogue : MonoBehaviour {

    public TalkTree dialogue;
	public GameObject npc;
    public Text npcText;
    public Text[] choices;
    public Image potrait;
    public UnityEvent[] dialogueEvent;
    public delegate void DialogueEnd();
    public DialogueEnd signal;

    public void UpdateDialogue()
    {
        Debug.Log("Dialogue State: " + dialogue.CurrentState);
        TalkState s = dialogue.State;

        if (dialogueEvent[dialogue.CurrentState] != null)
        {
            dialogueEvent[dialogue.CurrentState].Invoke();
        }


        if (s.endTree)
        {
            signal();
            Destroy(gameObject);
        }
        npcText.text = s.text;
        for (int i = 0; i < choices.Length; i++)
        {
			// Checking for dialogue condition
			// Default to true
			bool showOption = true;

			DialogueCondition dc = s.choices[i].dialogCondition;
			if (dc != null) {
				showOption = dc.Check (npc);

				Debug.Log ("Found a condition!");
				Debug.Log (showOption);
			}
				
			// Checks to see if the option is available
			if (s.choices[i].nextState == -1 || showOption == false) {
				// Disabling the button
				choices[i].transform.parent.gameObject.SetActive (false);
			} else {
				// Enables the button
				choices[i].transform.parent.gameObject.SetActive (true);
			}

			// Sets the text of the selection
            choices[i].text = s.choices[i].response;
        }
    }

    public void OnDialogueChoice(int c)
    {
		// Looking for a Dialogue Action to perform
		DialogueAction[] da = dialogue.states [dialogue.CurrentState].choices [c].dialogActions;

		if (da != null) {
			foreach (DialogueAction a in da) {
				a.Activate (npc);
			}
		}

        dialogue.Advance(c);
        UpdateDialogue();
    }
}
