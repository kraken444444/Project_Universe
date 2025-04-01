using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// The core dialogue node class
[Serializable]
public class DialogueNode
{
    public string nodeID;
    public string speakerName;
    [TextArea(3, 10)]
    public string dialogueText;
    public List<DialogueChoice> choices = new List<DialogueChoice>();
    public UnityEvent onNodeEnter;
    public UnityEvent onNodeExit;
    
    // For nodes that don't have choices (auto-progress)
    public string nextNodeID;
    
    // Optional conditions for node availability
    public bool hasCondition;
    public string conditionName;
}
