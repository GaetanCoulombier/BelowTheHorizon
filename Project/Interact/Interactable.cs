using Godot;

namespace BelowTheHorizon{
    
public interface Interactable
{
    public void Interact(PlayerController player);
    public bool CanInteract(PlayerController player);
    public string GetPromptMessage();
    public string GetPromptErrorMessage();
    public string GetInteractAction();
}

}