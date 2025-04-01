using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Tree", menuName = "Dialogue System/Dialogue Tree")]
public class DialogueTree : ScriptableObject
{
    public List<DialogueNode> nodes = new List<DialogueNode>();
    public string startNodeID;
    
    // Helper method to get a node by ID
    public DialogueNode GetNodeByID(string id)
    {
        return nodes.Find(node => node.nodeID == id);
    }
}
