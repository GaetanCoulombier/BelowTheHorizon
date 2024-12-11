using Godot;
using System;

public partial class Camera : Camera3D
{
    private Node3D _player;
    private Vector3 _offset = new Vector3(0, 3, 6);
    private Vector2 _rotation = Vector2.Zero;
    private float _sensitivity = 0.1f; // TODO : Add this to the settings menu
    private float _maxVerticalAngle = 45;

    public override void _Ready()
    {
        _player = GetNode<Node3D>("/root/GameRoot/Player");
        
        // TODO : Change this based on wether the game is paused or not
        Input.MouseMode = Input.MouseModeEnum.Captured; // Hide the mouse cursor
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_player != null)
        {
            UpdateCameraPosition();
        }
    }

    // Update the camera position based on the player's position and rotation
    private void UpdateCameraPosition()
    {
        Vector3 targetPosition = _player.GlobalTransform.Origin;
        Basis rotationBasis = CalculateRotationBasis();
        Vector3 rotatedOffset = rotationBasis * _offset;
        GlobalPosition = targetPosition + rotatedOffset;
        LookAt(targetPosition, Vector3.Up);
    }

    private Basis CalculateRotationBasis()
    {
        return new Basis(new Vector3(0, 1, 0), Mathf.DegToRad(_rotation.Y))
             * new Basis(new Vector3(1, 0, 0), Mathf.DegToRad(_rotation.X));
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        HandleMouseInput(@event);
    }

    private void HandleMouseInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseEvent)
        {
            // Modify the camera rotation based on the mouse movement
            Vector2 mouseDelta = mouseEvent.Relative;
            _rotation.X -= mouseDelta.Y * _sensitivity;
            _rotation.Y -= mouseDelta.X * _sensitivity;

            // Clamp the vertical rotation angle to prevent the camera from flipping
            _rotation.X = Mathf.Clamp(_rotation.X, -_maxVerticalAngle, _maxVerticalAngle);
        }
    }
}
