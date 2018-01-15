using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct TalkChoice // What the player says/does
{
    public string response;
    public int nextState;
	public DialogueAction[] dialogActions;
	public DialogueCondition dialogCondition;
}

[System.Serializable]
public struct TalkState // What the NPC says/does
{
    public string text;
    public TalkChoice[] choices;
    public bool endTree;
}

[System.Serializable]
[CreateAssetMenu(fileName = "TalkTree", menuName = "Dialogue/TalkTree")]
public class TalkTree : ScriptableObject
{
    public TalkState[] states;
    private int currentState = 0;

    public TalkState Advance(int n)
    {
        int next = states[currentState].choices[n].nextState;
        currentState = next >= 0 ? next : currentState;
        return states[currentState];
    }

    public TalkState State
    {
        get { return states[currentState]; }
    }

    public int CurrentState
    {
        get { return currentState; }
    }

    public void Reset()
    {
        currentState = 0;
    }
}
