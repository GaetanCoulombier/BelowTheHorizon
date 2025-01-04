using Godot;

public partial class DetectionController : Node3D
{
    /* Componants */
    [Export] private PlayerController _playerController;
    [Export] private RayCast3D _rayCastFacing;
    [Export] private RayCast3D _rayCastClimbUp;

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_rayCastFacing.IsColliding()){
            if (_rayCastClimbUp.IsColliding() && !_playerController.IsOnFloor()){
                _playerController.SetMovementType(MovementType.CLIMBING);
            } else {
                // TODO : Make the player climb up the surface
                _playerController.SetMovementType(MovementType.GROUND);
            }
        }
        else {
            _playerController.SetMovementType(MovementType.GROUND);
        }
    }
}