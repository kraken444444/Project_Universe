using UnityEngine;

public class DialogueNPC : MonoBehaviour
{
    public DialogueTree dialogueTree;
    public float interactionRadius = 3f;
    private SphereCollider interactionCollider;
    private bool playerInRange = false;
    
    private void Awake()
    {
        // Create and configure the sphere collider
        interactionCollider = gameObject.AddComponent<SphereCollider>();
        interactionCollider.radius = interactionRadius;
        interactionCollider.isTrigger = true;
    }
    
    private void Update()
    {
        // Start dialogue when E is pressed and player is in range
        if (playerInRange)
        {
            DialogueManager manager = FindObjectOfType<DialogueManager>();
            if (manager != null)
            {
                manager.StartDialogue(dialogueTree);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    
    // Visualize the interaction radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
