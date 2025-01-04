using Godot;

// Handle every interactions that the player can do with the environment (with hands and tools)
public partial class InteractionController : RayCast3D
{
    /* Nodes */
    [Export] private QuickAccessBar _quickAccessBar;
    [Export] private PlayerController _player;
    [Export] private Control _uiInteractPrompt;
    private Label prompt;

    /* Variables */


    /* Godot methods */
    public override void _Ready()
    {
        prompt = _uiInteractPrompt.GetNode<Label>("Label");
    }

    // Manage and print interactions with the environment
    public override void _Process(double delta)
    {
        if (IsColliding())
        {
            var collidingObject = GetCollider();
            if (collidingObject is BTHInteractable interactable)
            {
                // Show interactable action on the screen
                _uiInteractPrompt.Visible = true;
                prompt.Text = interactable.GetPromptMessage();

                if (Input.IsActionJustPressed(interactable.GetInteractAction()))
                    interactable.Interact(_player);
                return;
            }
        }

        // Hide prompt if not interacting with a valid object
        _uiInteractPrompt.Visible = false;
        prompt.Text = "";
    }





    /* Manage current tool interactions */
    // Click droit
    // Click gauche
    // Option suplémentaire (ex : changer les piles)
    // Drop de l'item courrent

    public override void _Input(InputEvent @event)
    {   
        if (GameState.isActionsBlocked) 
            return;

        var slot = _quickAccessBar.GetCurrentSelectedSlot();
        if (slot == null)
            return;

        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {

            // Get the item in the player hand
            var item = slot.GetItem();

            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                item.UsePrimary(_player);
            }
            else if (mouseEvent.ButtonIndex == MouseButton.Right)
            {
                item.UseSecondary(_player);
            }
        }

        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (Input.IsActionJustPressed("drop_item"))
            {
                slot.Drop();
            }
        }
    }
}