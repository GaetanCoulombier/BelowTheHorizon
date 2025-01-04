using Godot;

public abstract partial class BTHInteractable : Node3D
{
    [Export] private string messagePrompt = "do somthing";
    [Export] private string interactAction = "interact";

    public abstract void Interact(PlayerController player);

    public string GetPromptMessage()
    {
        return "To " + messagePrompt + " press" + "\n[" + GetInteractKey() + "]";
    }

    public string GetInteractAction()
    {
        return interactAction;
    }

    public string GetInteractKey()
    {
        return Utils.GetKeyByAction(interactAction);
    }

    protected void SetInteractAction(string action)
    {
        interactAction = action;
    }

    protected void SetPromptMessage(string message)
    {
        messagePrompt = message;
    }
}