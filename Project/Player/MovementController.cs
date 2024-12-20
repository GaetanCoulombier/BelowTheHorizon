using Godot;
using System;

public partial class MovementController
{
    private float _gravity = -9.81f;
    private float _jumpForce = 5.0f;

    private Vector3 _velocity = Vector3.Zero;
    


    private PlayerController _playerController;
    private Camera3D _camera;

    public MovementController(PlayerController playerController, Camera3D camera)
    {
        this._playerController = playerController;
        _camera = camera;

        _playerController.Connect(nameof(PlayerController.SetMovementStateEventHandler), this, nameof(OnMovementStateChange));
    }

    public void Handle(double delta, Vector3 direction)
    {
        // camera
        AlignPlayerToCamera();


        // movement
        _playerController.Velocity = _velocity;
        _playerController.MoveAndSlide();
    }




    /* Movement */
    public void OnInputChanged(Vector3 direction)
    {
        Vector3 possibleDirections = _playerController.movementState.PossibleDirections;
        Vector3 currentVelocity = _velocity;

        GD.Print("Direction: " + direction);
        GD.Print("Possible Directions: " + possibleDirections);
        
        if (direction == Vector3.Zero || possibleDirections == Vector3.Zero)
        {
            var inverse = new Vector3(1 - possibleDirections.X, 1 - possibleDirections.Y, 1 - possibleDirections.Z);
            _velocity *= inverse;
            return;
        }

        Basis cameraBasis = _camera.GlobalTransform.Basis;
        Vector3 forward = cameraBasis.Z.Normalized();
        Vector3 right = cameraBasis.X.Normalized();
        Vector3 up = cameraBasis.Y.Normalized();

        Vector3 movement = (right * direction.X + up * direction.Y + forward * direction.Z).Normalized();
        movement *= possibleDirections;

        // Appliquer la vitesse
        if (possibleDirections.X == 1) _velocity.X = movement.X * _playerController.movementState.MovementSpeed;
        if (possibleDirections.Y == 1) _velocity.Y = movement.Y * _playerController.movementState.MovementSpeed;
        if (possibleDirections.Z == 1) _velocity.Z = movement.Z * _playerController.movementState.MovementSpeed;
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
    if (_playerController.IsOnFloor())
    {
        _velocity.Y = 0;
    }
    else
    {
        _velocity.Y += _gravity * (float)delta;
    }
}





    /* Camera */
    public void AlignPlayerToCamera()
    {
        Vector3 directionToCamera = _camera.GlobalPosition - _playerController.GlobalTransform.Origin;
        directionToCamera.Y = 0;
        directionToCamera = directionToCamera.Normalized();
        _playerController.LookAt(_playerController.GlobalTransform.Origin - directionToCamera, Vector3.Up);
    }
}