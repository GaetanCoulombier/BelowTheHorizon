using Godot;
using System;
using System.Collections.Generic;

public partial class MovementController : Node
{
    /* Nodes */
    [Export] private PlayerController _player;
    [Export] private Node3D _meshRoot;
    [Export] private CameraController _camera;
    
    /* Movement variables */
    private Vector3 _possibleDirections = Vector3.Zero;
    private Vector3 _velocity = Vector3.Zero;
    private Vector3 _rawDirection = Vector3.Zero;
    private Vector3 _rotatedDirection = Vector3.Zero;
    private float _playerInitRotation;
    private float _cameraRotation;

    /* Settings */
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
        _camera = _player.GetNode<CameraController>("CamRoot");

        _playerInitRotation = _player.Rotation.Y;

        // TODO : Do it better
        OnChangeMovementState(_player.movementState);

        // Connect the signals
        _player.Connect(nameof(PlayerController.ChangeMovementState), new Callable(this, nameof(OnChangeMovementState)));
        _player.Connect(nameof(PlayerController.ChangeInput), new Callable(this, nameof(OnChangeInput)));
        _player.Connect(nameof(PlayerController.Jump), new Callable(this, nameof(OnJump)));
        _player.Connect(nameof(PlayerController.Fall), new Callable(this, nameof(OnFall)));
        _camera.Connect(nameof(CameraController.ChangeCameraRotation), new Callable(this, nameof(OnCameraUpdate)));
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
        if (_rotatedDirection == Vector3.Zero && !_player.IsOnFloor()) {
            if (_possibleDirections.X == 1) _velocity.X *= _possibleDirections.X * _momentum;
            if (_possibleDirections.Y == 1) _velocity.Y *= _possibleDirections.Y * _momentum;
            if (_possibleDirections.Z == 1) _velocity.Z *= _possibleDirections.Z * _momentum;
            return;
        }

        // If the player is not moving or the player can't move in the direction stop the player movement in the inversed possible directions
        if (_rotatedDirection == Vector3.Zero || _possibleDirections == Vector3.Zero) {
            var inversePossibleDirections = Vector3.One - _possibleDirections;
            _velocity *= inversePossibleDirections;
            return;
        }

        // Apply the movement
        if (_possibleDirections.X == 1) _velocity.X = _rotatedDirection.X * _speed;
        if (_possibleDirections.Y == 1) _velocity.Y = _rotatedDirection.Y * _speed;
        if (_possibleDirections.Z == 1) _velocity.Z = _rotatedDirection.Z * _speed;

        // Smoothly interpolate the current mesh rotation towards the target rotation
        float targetRotation = Mathf.Atan2(_rotatedDirection.X, _rotatedDirection.Z) - _playerInitRotation;
        float currentRotation = _meshRoot.Rotation.Y;
        _meshRoot.Rotation = new Vector3(
            _meshRoot.Rotation.X,
            Mathf.LerpAngle(currentRotation, targetRotation, (float) delta * _rotationSpeed),
            _meshRoot.Rotation.Z
        );
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
        _rawDirection = direction;
        _rotatedDirection = direction.Rotated(Vector3.Up, _cameraRotation);
    }

    public void OnJump()
    {
        _velocity.Y = _jumpForce;
    }

    public void OnFall()
    {
        _velocity.Y += _gravity * (float) GetProcessDeltaTime();
    }

    public void OnCameraUpdate(float cameraRotation)
    {
        _cameraRotation = cameraRotation;
        OnChangeInput(_rawDirection);
    }
}