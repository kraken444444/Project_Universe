#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueTree))]
public class DialogueTreeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogueTree tree = (DialogueTree)target;
        
        EditorGUILayout.LabelField("Dialogue Tree", EditorStyles.boldLabel);
        
        // Start node selection
        string[] nodeIDs = new string[tree.nodes.Count];
        for (int i = 0; i < tree.nodes.Count; i++)
        {
            nodeIDs[i] = tree.nodes[i].nodeID;
        }
        
        int startNodeIndex = Array.IndexOf(nodeIDs, tree.startNodeID);
        startNodeIndex = EditorGUILayout.Popup("Start Node", startNodeIndex, nodeIDs);
        if (startNodeIndex >= 0 && startNodeIndex < nodeIDs.Length)
        {
            tree.startNodeID = nodeIDs[startNodeIndex];
        }
        
        EditorGUILayout.Space();
        
        // Node list
        for (int i = 0; i < tree.nodes.Count; i++)
        {
            DialogueNode node = tree.nodes[i];
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField($"Node {i+1}: {node.nodeID}", EditorStyles.boldLabel);
            
            node.nodeID = EditorGUILayout.TextField("Node ID", node.nodeID);
            node.speakerName = EditorGUILayout.TextField("Speaker", node.speakerName);
            node.dialogueText = EditorGUILayout.TextArea(node.dialogueText, GUILayout.Height(60));
            
            // Choices
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Choices", EditorStyles.boldLabel);
            
            for (int j = 0; j < node.choices.Count; j++)
            {
                DialogueChoice choice = node.choices[j];
                
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                choice.choiceText = EditorGUILayout.TextField("Choice Text", choice.choiceText);
                
                int nextNodeIndex = Array.IndexOf(nodeIDs, choice.nextNodeID);
                nextNodeIndex = EditorGUILayout.Popup("Next Node", nextNodeIndex, nodeIDs);
                if (nextNodeIndex >= 0 && nextNodeIndex < nodeIDs.Length)
                {
                    choice.nextNodeID = nodeIDs[nextNodeIndex];
                }
                
                choice.hasCondition = EditorGUILayout.Toggle("Has Condition", choice.hasCondition);
                if (choice.hasCondition)
                {
                    choice.conditionName = EditorGUILayout.TextField("Condition", choice.conditionName);
                }
                
                if (GUILayout.Button("Remove Choice"))
                {
                    node.choices.RemoveAt(j);
                    j--;
                }
                
                EditorGUILayout.EndVertical();
            }
            
            if (GUILayout.Button("Add Choice"))
            {
                node.choices.Add(new DialogueChoice());
            }
            
            // Auto-progress node (when no choices)
            EditorGUILayout.Space();
            if (node.choices.Count == 0)
            {
                EditorGUILayout.LabelField("Auto Progress (No Choices)", EditorStyles.boldLabel);
                
                int nextNodeIndex = Array.IndexOf(nodeIDs, node.nextNodeID);
                nextNodeIndex = EditorGUILayout.Popup("Next Node", nextNodeIndex, nodeIDs);
                if (nextNodeIndex >= 0 && nextNodeIndex < nodeIDs.Length)
                {
                    node.nextNodeID = nodeIDs[nextNodeIndex];
                }
            }
            
            // Node conditions
            EditorGUILayout.Space();
            node.hasCondition = EditorGUILayout.Toggle("Has Condition", node.hasCondition);
            if (node.hasCondition)
            {
                node.conditionName = EditorGUILayout.TextField("Condition", node.conditionName);
            }
            
            // Remove node button
            if (GUILayout.Button("Remove Node"))
            {
                tree.nodes.RemoveAt(i);
                i--;
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }
        
        // Add node button
        if (GUILayout.Button("Add Node"))
        {
            DialogueNode newNode = new DialogueNode();
            newNode.nodeID = "Node_" + tree.nodes.Count;
            tree.nodes.Add(newNode);
        }
        
        // Save changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
#endif