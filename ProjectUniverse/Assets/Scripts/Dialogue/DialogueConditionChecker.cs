using System.Collections.Generic;
using UnityEngine;

public class DialogueConditionChecker : MonoBehaviour, IDialogueCondition
{
    // Store your game variables/flags here
    public Dictionary<string, bool> gameFlags = new Dictionary<string, bool>();
    
    public bool CheckCondition(string conditionName)
    {
        if (string.IsNullOrEmpty(conditionName))
            return true;
            
        if (gameFlags.TryGetValue(conditionName, out bool result))
            return result;
            
        return false;
    }
    
    // Methods to set flags for quest progression, etc.
    public void SetFlag(string flagName, bool value)
    {
        gameFlags[flagName] = value;
    }
}

