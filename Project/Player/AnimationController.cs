

using Godot;

public partial class AnimationController : Node
{
    /* Componants */
    private PlayerController _playerController;
    private Node3D _rootMesh;

    public override void _Ready()
    {
        _playerController = GetParent<PlayerController>();
    }

    public override void _PhysicsProcess(double delta)
    {
        
    }
}