using System;
using Godot;

public partial class MovementController : Node
{
    /* Nodes */
    [Export] private PlayerController _player;
    [Export] private Node3D _head;
    [Export] private RayCast3D _facingCheck;
    [Export] private RayCast3D _leftFacingCheck;
    [Export] private RayCast3D _rightFacingCheck;
    [Export] private RayCast3D _leftSurfaceCheck;
    [Export] private RayCast3D _rightSurfaceCheck;
    [Export] private RayCast3D _leftCheck;
    [Export] private RayCast3D _rightCheck;

    /* Movement variables */
    private Vector3 _velocity = Vector3.Zero;
    private Vector3 _direction = Vector3.Zero;
    private float _speed = 0.0f;

    /* Settings */
    [Export] private float GRAVITY = -9.8f;
    [Export] private float JUMP_FORCE = 3.0f;
    [Export] private float MOMENTUM = 0.98f;
    


    /* Godot methods */
    public override void _Ready()
    {
        // Connect the signals
        _player.Connect(nameof(PlayerController.ChangeMovementState), new Callable(this, nameof(OnChangeMovementState)));
        _player.Connect(nameof(PlayerController.ChangeInput), new Callable(this, nameof(OnChangeInput)));
        _player.Connect(nameof(PlayerController.Jump), new Callable(this, nameof(OnJump)));
        _player.Connect(nameof(PlayerController.Fall), new Callable(this, nameof(OnFall)));
    }

    public override void _PhysicsProcess(double delta)
    {
        // Update the movement based on the current movement type
        switch (_player.movementType)
        {
            case MovementType.GROUND:
                UpdateGroundMovement(delta);
                break;
            case MovementType.CLIMBING:
                UpdateClimbMovement(delta);
                break;
            case MovementType.SWIMMING:
                UpdateSwimMovement(delta);
                break;
            default:
                GD.PrintErr("Movement state not implemented");
                break;
        }

        // Apply the velocity to the player
        _player.SetVelocity(_velocity);
        _player.MoveAndSlide();
    }



    /* Custom methods */
    private void UpdateGroundMovement(double delta)
    {
        var rotatedDirection = _direction.Rotated(Vector3.Up, _head.Rotation.Y);

        if (_player.IsOnFloor()) {
            if (rotatedDirection == Vector3.Zero) {
                _velocity.X = _velocity.Z = 0;
            } else {
                _velocity.X = rotatedDirection.X * _speed;
                _velocity.Z = rotatedDirection.Z * _speed;
            }
        } else {
            _velocity.X = Mathf.Lerp(_velocity.X, rotatedDirection.X * _speed, (float) delta * MOMENTUM);
            _velocity.Z = Mathf.Lerp(_velocity.Z, rotatedDirection.Z * _speed, (float) delta * MOMENTUM);
        }
    }

    private void UpdateClimbMovement(double delta)
    {
        var rotatedDirection = _direction.Rotated(Vector3.Up, _head.Rotation.Y);
        // TODO : Monter en haut de la surface
        // TODO : Implementer le hanging
        // TODO : vérifier les murs adjacents pour pouvoir tourner si il en exisent
        // TODO : Régler le problème de poussé en diagonal lorsque le joueur monte ou descends une surface en dévers

        GD.Print("rightSurfaceCheck: " + _rightSurfaceCheck.IsColliding());
        GD.Print("leftSurfaceCheck: " + _leftSurfaceCheck.IsColliding());
        GD.Print("rightCheck: " + _rightCheck.IsColliding());
        GD.Print("leftCheck: " + _leftCheck.IsColliding());

        var surfaceNormal = _facingCheck.GetCollisionNormal();

        // Check if the player is at the edge of the surface
        if (!_leftFacingCheck.IsColliding() && _direction.X > 0) _direction.X = 0;
        if (!_rightFacingCheck.IsColliding() && _direction.X < 0) _direction.X = 0;

        // Go around inner corners
        if (_leftCheck.IsColliding() && _direction.X > 0) surfaceNormal = _leftCheck.GetCollisionNormal();
        if (_rightCheck.IsColliding() && _direction.X < 0) surfaceNormal = _rightCheck.GetCollisionNormal();

        GD.Print("Surface normal: " + surfaceNormal);

        // Get the tangent of the surface
        var tangent = surfaceNormal.Cross(Vector3.Up).Normalized();
        if (tangent == Vector3.Zero) tangent = Vector3.Right;

        // Get the local direction of the player based on the surface
        var localDirection = (_direction * new Basis(tangent, surfaceNormal.Cross(tangent).Normalized(), surfaceNormal)).Normalized();
        if (localDirection == Vector3.Zero)
        {
            _velocity = _direction = Vector3.Zero;
            return;
        }

        // Apply the movement
        _velocity = localDirection * _speed;

        // Push the player towards the surface (to prevent detachment)
        Vector3 pushToSurface = surfaceNormal * -2.0f;
        _velocity += pushToSurface;
    }

    private void UpdateSwimMovement(double delta)
    {
        throw new NotImplementedException();
    }




    /* Signals */
    public void OnChangeMovementState(MovementState movementState)
    {
        _speed = movementState.speed;
    }

    public void OnChangeInput(Vector3 direction)
    {
        _direction = direction;
    }

    public void OnJump()
    {
        _velocity.Y = JUMP_FORCE;
    }

    public void OnFall()
    {
        _velocity.Y += GRAVITY * (float) GetProcessDeltaTime();
    }
}