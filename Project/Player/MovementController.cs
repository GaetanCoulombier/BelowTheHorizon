using Godot;
using System;
using System.Collections.Generic;

public partial class MovementController : Node
{
    /* Nodes */
    private PlayerController _player;
    private Node3D _meshRoot;
    private Node3D _cameraRoot;
    
    /* Movement variables */
    private Vector3 _possibleDirections = Vector3.Zero;
    private Vector3 _velocity = Vector3.Zero;
    private Vector3 _direction = Vector3.Zero;
    private float _playerInitRotation;
    [Export] private float _speed = 0.0f;
    [Export] private float _acceleration = 0.0f;
    [Export] private float _rotationSpeed = 8.0f;
    [Export] private float _gravity = -9.81f;
    [Export] private float _jumpForce = 5.0f;
    [Export] private float _momentum = 0.98f;



    /* Godot methods */
    public override void _Ready()
    {
        _player = GetParent<PlayerController>();
        _meshRoot = _player.GetNode<Node3D>("MeshRoot");
        _cameraRoot = _player.GetNode<Node3D>("CamRoot");

        _playerInitRotation = _player.Rotation.Y;

        OnChangeMovementState(_player.movementState);

        // Connect the signals
        _player.Connect(nameof(PlayerController.ChangeMovementState), new Callable(this, nameof(OnChangeMovementState)));
        _player.Connect(nameof(PlayerController.ChangeInput), new Callable(this, nameof(OnChangeInput)));
        _player.Connect(nameof(PlayerController.Jump), new Callable(this, nameof(OnJump)));
        _player.Connect(nameof(PlayerController.Fall), new Callable(this, nameof(OnFall)));
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateMovement(delta);

        // Apply the velocity to the player
        _player.SetVelocity(_velocity);
        _player.MoveAndSlide();
    }
    
    /* Custom methods */
    private void UpdateMovement(double delta)
    {
        // Momentum if the player is in the air and not touching ground
        if (_direction == Vector3.Zero && !_player.IsOnFloor()) {
            if (_possibleDirections.X == 1) _velocity.X *= _possibleDirections.X * _momentum;
            if (_possibleDirections.Y == 1) _velocity.Y *= _possibleDirections.Y * _momentum;
            if (_possibleDirections.Z == 1) _velocity.Z *= _possibleDirections.Z * _momentum;
            return;
        }

        // If the player is not moving or the player can't move in the direction stop the player movement in the inversed possible directions
        if (_direction == Vector3.Zero || _possibleDirections == Vector3.Zero) {
            var inversePossibleDirections = Vector3.One - _possibleDirections;
            _velocity *= inversePossibleDirections;
            return;
        }

        // Align the movement with the camera
        Basis cameraBasis = _cameraRoot.GlobalTransform.Basis;
        Vector3 forward = cameraBasis.Z.Normalized();
        Vector3 right = cameraBasis.X.Normalized();
        Vector3 up = cameraBasis.Y.Normalized();

        Vector3 movement = (right * _direction.X + up * _direction.Y + forward * _direction.Z).Normalized();
        movement *= _possibleDirections;

        // Apply the movement
        if (_possibleDirections.X == 1) _velocity.X = movement.X * _speed;
        if (_possibleDirections.Y == 1) _velocity.Y = movement.Y * _speed;
        if (_possibleDirections.Z == 1) _velocity.Z = movement.Z * _speed;

        // Smoothly interpolate the current mesh rotation towards the target rotation
        float targetRotation = Mathf.Atan2(movement.X, movement.Z) - _playerInitRotation;
        float currentRotation = _meshRoot.Rotation.Y;
        _meshRoot.Rotation = new Vector3(
            _meshRoot.Rotation.X,
            Mathf.LerpAngle(currentRotation, targetRotation, (float) GetProcessDeltaTime() * _rotationSpeed),
            _meshRoot.Rotation.Z
        );

        _velocity.Lerp(_velocity, _acceleration * (float) delta);
    }


    /* Signals */
    public void OnChangeMovementState(MovementState state)
    {
        _speed = state.MovementSpeed;
        _acceleration = state.Acceleration;
        _possibleDirections = state.PossibleDirections;
    }

    public void OnChangeInput(Vector3 direction)
    {
        _direction = direction;
    }

    public void OnJump()
    {
        _velocity.Y = _jumpForce;
    }

    public void OnFall()
    {
        _velocity.Y += _gravity * (float) GetProcessDeltaTime();
    }
}