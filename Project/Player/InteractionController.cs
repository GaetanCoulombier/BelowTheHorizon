using Godot;

public partial class InteractionController : RayCast3D
{
    [Export] private PlayerController _player;
    [Export] private Control _uiInteractPrompt;
    private Label prompt;

    public override void _Ready()
    {
        prompt = _uiInteractPrompt.GetNode<Label>("Label");
    }

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
}