using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct TalkChoice // What the player says/does
{
    public string response;
    public int nextState;
}

[System.Serializable]
public struct TalkState // What the NPC says/does
{
    public string text;
    public TalkChoice[] choices;
    public string runMethod;
    public bool endTree;
}

[System.Serializable]
[CreateAssetMenu(fileName = "TalkTree", menuName = "Dialogue/TalkTree")]
public class TalkTree : ScriptableObject
{
    public int initialState;
    public TalkState[] states;
    private int currentState;

    public TalkState Advance(TalkChoice n)
    {
        currentState = n.nextState;
        return states[currentState];
    }

    public TalkState State
    {
        get { return states[currentState]; }
    }
}
