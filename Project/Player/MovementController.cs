using System;
using Godot;

public partial class MovementController : Node
{
    /* Nodes */
    [Export] private PlayerController _player;
    [Export] private Node3D _head;

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
        throw new NotImplementedException();
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