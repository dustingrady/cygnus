using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(fileName = "Container", menuName = "Dialogue/DelegateContainer")]
public class DelegateContainer : ScriptableObject {
    public UnityEvent dialogueEvent;
}
