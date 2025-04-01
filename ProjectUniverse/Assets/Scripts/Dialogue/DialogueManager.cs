using System.Collections.Generic;
using UnityEngine;
public class DialogueManager : MonoBehaviour
{
    public DialogueTree currentDialogue;
    public DialogueConditionChecker conditionChecker;
    
    // UI references - you'll need to assign these
    public GameObject dialoguePanel;
    public TMPro.TextMeshProUGUI speakerNameText;
    public TMPro.TextMeshProUGUI dialogueText;
    public Transform choicesContainer;
    public GameObject choiceButtonPrefab;
    
    private DialogueNode currentNode;
    private bool isDialogueActive = false;
    
    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
            
        if (conditionChecker == null)
            conditionChecker = GetComponent<DialogueConditionChecker>();
    }
    
    public void StartDialogue(DialogueTree dialogue)
    {
        if (dialogue == null) return;
        
        currentDialogue = dialogue;
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        
        SetCurrentNode(dialogue.startNodeID);
    }
    
    public void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        currentNode = null;
        currentDialogue = null;
    }
    
    public void SetCurrentNode(string nodeID)
    {
        if (currentDialogue == null) return;
        
        // Get the node
        currentNode = currentDialogue.GetNodeByID(nodeID);
        if (currentNode == null)
        {
            Debug.LogError($"Node with ID {nodeID} not found!");
            EndDialogue();
            return;
        }
        
        // Invoke the onNodeEnter event
        currentNode.onNodeEnter?.Invoke();
        
        // Update UI with node content
        UpdateDialogueUI();
    }
    
    private void UpdateDialogueUI()
    {
        if (currentNode == null) return;
        
        // Update speaker and dialogue text
        speakerNameText.text = currentNode.speakerName;
        dialogueText.text = currentNode.dialogueText;
        
        // Clear existing choice buttons
        foreach (Transform child in choicesContainer)
        {
            Destroy(child.gameObject);
        }
        
        // If there are choices, create choice buttons
        if (currentNode.choices.Count > 0)
        {
            foreach (DialogueChoice choice in currentNode.choices)
            {
                // Check if this choice is available based on conditions
                if (choice.hasCondition && conditionChecker != null && 
                    !conditionChecker.CheckCondition(choice.conditionName))
                    continue;
                
                // Create button
                GameObject choiceObj = Instantiate(choiceButtonPrefab, choicesContainer);
                TMPro.TextMeshProUGUI choiceText = choiceObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (choiceText != null)
                    choiceText.text = choice.choiceText;
                
                // Add click event
                UnityEngine.UI.Button button = choiceObj.GetComponent<UnityEngine.UI.Button>();
                if (button != null)
                {
                    string nextNodeID = choice.nextNodeID;
                    button.onClick.AddListener(() => {
                        // Exit current node
                        currentNode.onNodeExit?.Invoke();
                        // Move to next node
                        SetCurrentNode(nextNodeID);
                    });
                }
            }
        }
        // If there are no choices but there's a next node, add a "Continue" button
        else if (!string.IsNullOrEmpty(currentNode.nextNodeID))
        {
            GameObject choiceObj = Instantiate(choiceButtonPrefab, choicesContainer);
            TMPro.TextMeshProUGUI choiceText = choiceObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (choiceText != null)
                choiceText.text = "Continue";
            
            UnityEngine.UI.Button button = choiceObj.GetComponent<UnityEngine.UI.Button>();
            if (button != null)
            {
                string nextNodeID = currentNode.nextNodeID;
                button.onClick.AddListener(() => {
                    // Exit current node
                    currentNode.onNodeExit?.Invoke();
                    // Move to next node
                    SetCurrentNode(nextNodeID);
                });
            }
        }
        // If there are no choices and no next node, add an "End Conversation" button
        else
        {
            GameObject choiceObj = Instantiate(choiceButtonPrefab, choicesContainer);
            TMPro.TextMeshProUGUI choiceText = choiceObj.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (choiceText != null)
                choiceText.text = "Goodbye";
            
            UnityEngine.UI.Button button = choiceObj.GetComponent<UnityEngine.UI.Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => {
                    // Exit current node
                    currentNode.onNodeExit?.Invoke();
                    // End conversation
                    EndDialogue();
                });
            }
        }
    }
}
