using Godot;
using System;

public partial class MovementController
{
    private float _gravity = -9.81f;
    private float _jumpForce = 5.0f;

    private Vector3 _velocity = Vector3.Zero;
    private Vector3 _lastInputDirection = Vector3.Zero;


    private PlayerController _playerController;
    private Camera3D _camera;

    public MovementController(PlayerController playerController, Camera3D camera)
    {
        this._playerController = playerController;
        _camera = camera;
    }

    public void Handle(double delta, Vector3 direction)
    {
        _playerController.Velocity = _velocity;
        _playerController.MoveAndSlide();

        // Camera
        //AlignPlayerToCamera();
    }




    /* Movement */
    public void OnInputChanged(Vector3 direction)
    {
        Vector3 possibleDirections = _playerController.movementState.PossibleDirections;
        
        if (direction == Vector3.Zero || possibleDirections == Vector3.Zero)
        {   
            _velocity = Vector3.Zero;
            return;
        }

        Vector3 movement = direction * possibleDirections * _playerController.movementState.MovementSpeed;

        // Camera relative movement
        Basis cameraBasis = _camera.GlobalTransform.Basis;
        cameraBasis = new Basis(cameraBasis.X, Vector3.Up, cameraBasis.Z);
        Vector3 globalMovement = cameraBasis * movement;

        // Update the player's velocity
        _velocity = direction * globalMovement;
    }

    public void OnJump()
    {
        if (_playerController.IsOnFloor())
        {
            _velocity.Y += _jumpForce;
        }
    }

    public void OnFall(double delta)
    {
        if (!_playerController.IsOnFloor()) _velocity.Y += _gravity * (float)delta;
        else _velocity.Y = 0;
    }




    /* Camera */
    private void AlignPlayerToCamera()
    {
        Vector3 directionToCamera = _camera.GlobalPosition - _playerController.GlobalTransform.Origin;
        directionToCamera.Y = 0;
        directionToCamera = directionToCamera.Normalized();
        _playerController.LookAt(_playerController.GlobalTransform.Origin - directionToCamera, Vector3.Up);
    }
}