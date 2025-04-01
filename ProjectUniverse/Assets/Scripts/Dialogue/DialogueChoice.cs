using System;
using UnityEngine;
[Serializable]
public class DialogueChoice
{
    [TextArea(1, 3)]
    public string choiceText;
    public string nextNodeID;
    
    // Optional conditions for choice availability
    public bool hasCondition;
    public string conditionName;
}
