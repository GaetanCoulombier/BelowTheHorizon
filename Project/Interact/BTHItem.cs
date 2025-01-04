using Godot;

public abstract partial class BTHItem : BTHInteractable
{
    [Export] private Texture2D _icon;
    private int _maxStackSize = 1;//[Export] private int _maxStackSize = 1;
    private bool _inInventory = false;

    public override void _Ready()
    {
        SetPromptMessage("pick up");
    }

    public override void Interact(PlayerController player)
    {
        if (!_inInventory)
        {
            // Add to inventory
            // Remove from scene
            var parent = GetParent();

            bool added = player.GetInventory().AddItem(this);
            if (added) {
                parent.RemoveChild(this);
                _inInventory = true;
            } else {
                // Show message that inventory is full
            }
        }
    }

    public void Drop(PlayerController player)
    {
        if (_inInventory)
        {
            // Remove from inventory

            // Add to scene
            var parentPlayer = player.GetParent();
            parentPlayer.AddChild(this);
            // Set position to player position
            GlobalTransform = player.GlobalTransform;

            _inInventory = false;
        }
    }

    public Texture2D GetIcon()
    {
        return _icon;
    }

    public int GetMaxStackSize()
    {
        return _maxStackSize;
    }
}