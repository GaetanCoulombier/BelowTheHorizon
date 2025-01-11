using Godot;

namespace BelowTheHorizon {

public abstract partial class Item : RigidBody3D, Interactable
{
    [Export] private string messagePrompt = "pick upg";
    [Export] private string errorMessagePrompt = "Inventory is full";
    [Export] private string interactAction = "interact";
    [Export] private Texture2D _icon;

    public abstract void UsePrimary(PlayerController player);
    public abstract void UseSecondary(PlayerController player);

    public override void _Ready()
    {
        Freeze = true;
    }


    public virtual void Interact(PlayerController player)
    {
        // Add to the player inventory
        if (!CanInteract(player))
            return;
            
        var parent = GetParent();
        player.GetInventory().AddItem(this);
        parent.RemoveChild(this);
    }

    public virtual bool CanInteract(PlayerController player)
    {
        return player.GetInventory().CanAddItem(this);
    }

    public void Drop(PlayerController player)
    {
        // Add to scene
        var parentPlayer = player.GetParent();
        parentPlayer.AddChild(this);
        // Set position to player position
        GlobalTransform = player.GlobalTransform;

        Freeze = false;
    }

    public Texture2D GetIcon()
    {
        return _icon;
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