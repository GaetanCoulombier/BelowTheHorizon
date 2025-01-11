using Godot;

namespace BelowTheHorizon {
public partial class Object : StaticBody3D, Interactable
{
    [Export] private string messagePrompt = "do something";
    [Export] private string errorMessagePrompt = "Can't do that";
    [Export] private string interactAction = "interact";

    public virtual void Interact(PlayerController player)
    {
        GD.Print("Interacting with object");
    }

    public virtual bool CanInteract(PlayerController player)
    {
        return true;
    }

    public string GetPromptMessage()
    {
        return "To " + messagePrompt + " press" + "\n[" + GetInteractKey() + "]";
    }

    public string GetPromptErrorMessage()
    {
        return errorMessagePrompt;
    }

    public string GetInteractKey()
    {
        return Utils.GetKeyByAction(interactAction);
    }

    public string GetInteractAction()
    {
        return interactAction;
    }
}

}