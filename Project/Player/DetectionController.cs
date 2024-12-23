using Godot;

public partial class DetectionController : Node3D
{
    /* Componants */
    private PlayerController _playerController;
    private RayCast3D _rayCastFacing;
    private RayCast3D _rayCastClimbUp;

    public override void _Ready()
    {
        _playerController = GetParent<Node3D>().GetParent<PlayerController>();
        _rayCastFacing = GetNode<RayCast3D>("Climbing/RayCastFacing");
        _rayCastClimbUp = GetNode<RayCast3D>("Climbing/RayCastClimbUp");

    }

    public override void _PhysicsProcess(double delta)
    {
        if (_rayCastFacing.IsColliding()){
            if (_rayCastClimbUp.IsColliding() && !_playerController.IsOnFloor()){
                _playerController.SetMovementType(MovementType.CLIMBING);
            } else {
                _playerController.SetMovementType(MovementType.GROUND);
            }
        }
        else {
            _playerController.SetMovementType(MovementType.GROUND);
        }
    }
}