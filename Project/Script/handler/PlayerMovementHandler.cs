using System;
using Godot;

public class PlayerMovementHandler
{
    private CharacterBody3D _player; // Référence au joueur
    private Camera3D _camera; // Référence à la caméra du joueur
    private float _speed; // Vitesse du joueur
    private float _gravity; // Gravité
    private float _jumpStrength; // Force de saut
    private Vector3 _velocity; // Vélocité du joueur

    public PlayerMovementHandler(CharacterBody3D player, Camera3D camera, float speed, float gravity, float jumpStrength)
    {
        _player = player;
        _camera = camera;
        _speed = speed;
        _gravity = gravity;
        _jumpStrength = jumpStrength;
        _velocity = Vector3.Zero;
    }

    public void HandleMovement(double delta)
    {
        AlignPlayerToCamera();

        ApplyGravity(delta);

        Vector3 inputMovement = GetInputMovement();
        Vector3 horizontalMovement = new Vector3(inputMovement.X, 0, inputMovement.Z) * _speed;

        // Camera relative movement
        Vector3 globalMovement = TransformDirection(horizontalMovement);

        // Update the player's velocity
        _velocity.X = globalMovement.X;
        _velocity.Z = globalMovement.Z;

        // Move the player
        _player.Velocity = _velocity;
        _player.MoveAndSlide();
    }

    private void AlignPlayerToCamera()
    {
        Vector3 directionToCamera = _camera.GlobalPosition - _player.GlobalTransform.Origin;
        directionToCamera.Y = 0;
        directionToCamera = directionToCamera.Normalized();
        _player.LookAt(_player.GlobalTransform.Origin - directionToCamera, Vector3.Up);
    }

    private void ApplyGravity(double delta)
    {
        if (!_player.IsOnFloor())
        {
            _velocity.Y += _gravity * (float)delta; // Apply gravity to the vertical velocity
        }
        else if (_velocity.Y < 0)
        {
            _velocity.Y = 0; // Reset the vertical velocity when on the floor
        }
    }

    private Vector3 GetInputMovement()
    {
        Vector3 inputVector = Vector3.Zero;

        if (Input.IsActionPressed("move_forward")) inputVector.Z -= 1;
        if (Input.IsActionPressed("move_backward")) inputVector.Z += 1;
        if (Input.IsActionPressed("move_left")) inputVector.X -= 1;
        if (Input.IsActionPressed("move_right")) inputVector.X += 1;
        if (Input.IsActionJustPressed("jump") && _player.IsOnFloor()) _velocity.Y = _jumpStrength;

        // Normalize the input vector to prevent faster diagonal movement
        return inputVector.Normalized();
    }

    // Transform a local direction to a global direction based on the camera's orientation to make the player move relative to the camera
    private Vector3 TransformDirection(Vector3 localDirection)
    {
        Basis cameraBasis = _camera.GlobalTransform.Basis;
        cameraBasis = new Basis(cameraBasis.X, Vector3.Up, cameraBasis.Z);
        return cameraBasis * localDirection;
    }
}
