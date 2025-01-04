using Godot;

public partial class PlayerController : CharacterBody3D
{
    /* Nodes */
    [Export] private Inventory _inventory;

    /* Godot methods */
    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
    }
    
    public Inventory GetInventory()
    {
        return _inventory;
    }
}