using UnityEngine;

public interface IInteract
{
    // Display name of the interaction that will appear in UI prompts
    string InteractionPrompt { get; }

    //  the object can currently be interacted with
    bool CanInteract { get; }

    // called when player interacts with this object
    void Interact(GameObject interactibleObject);



}

