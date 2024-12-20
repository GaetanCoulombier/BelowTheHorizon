using Godot;
using System;

public partial class MovementController : Node
{
    /* Nodes */
    private PlayerController _player;
    private Node3D _meshRoot;
    private Node3D _cameraRoot;
    
    /* Movement variables */
    private Vector3 _velocity = Vector3.Zero;
    private Vector3 _direction = Vector3.Zero;
    
    private float _playerInitRotation;


    /* Movement configuration */
    public MovementController(PlayerController playerController, Node3D meshRoot, Node3D cameraRoot)
    {
        this._player = playerController;
        this._meshRoot = meshRoot;
        this._cameraRoot = cameraRoot;

        _playerInitRotation = playerController.Rotation.Y;

        Connect(nameof(PlayerController.PlayerInputChangedEventHandler), new Callable(this, nameof(OnInputChanged)));
    }

    public void Handle(double delta)
    {   
        // Apply the player's movement and player's rotation
        HandleMovement(delta);

        // Move the player
        _player.Velocity = _velocity;
        _player.MoveAndSlide();
    }

    private void HandleMovement(double delta)
    {
        Vector3 possibleDirections = _player.movementState.PossibleDirections;

        // If the player is not moving or the player can't move in the direction stop the player movement in the inversed possible directions
        if (_direction == Vector3.Zero || possibleDirections == Vector3.Zero)
        {
            // TODO : Change the logique to add a momentum to the player
            var inversePossibleDirections = new Vector3(1 - possibleDirections.X, 1 - possibleDirections.Y, 1 - possibleDirections.Z);
            _velocity *= inversePossibleDirections;
            return;
        }

        // Align the movement with the camera
        Basis cameraBasis = _cameraRoot.GlobalTransform.Basis;
        Vector3 forward = cameraBasis.Z.Normalized();
        Vector3 right = cameraBasis.X.Normalized();
        Vector3 up = cameraBasis.Y.Normalized();

        Vector3 movement = (right * _direction.X + up * _direction.Y + forward * _direction.Z).Normalized();
        movement *= possibleDirections;

        // Apply the movement
        if (possibleDirections.X == 1) _velocity.X = movement.X * _player.movementState.MovementSpeed;
        if (possibleDirections.Y == 1) _velocity.Y = movement.Y * _player.movementState.MovementSpeed;
        if (possibleDirections.Z == 1) _velocity.Z = movement.Z * _player.movementState.MovementSpeed;

        // Smoothly interpolate the current mesh rotation towards the target rotation
        float targetRotation = Mathf.Atan2(movement.X, movement.Z) - _playerInitRotation;
        float currentRotation = _meshRoot.Rotation.Y;
        _meshRoot.Rotation = new Vector3(
            _meshRoot.Rotation.X,
            Mathf.LerpAngle(currentRotation, targetRotation, (float)delta * _player.rotationSpeed),
            _meshRoot.Rotation.Z
        );
    }



    /* Signals */
    public void OnInputChanged(Vector3 direction)
    {
        _direction = direction;
    }

    public void OnJump()
    {
        if (_player.IsOnFloor())
        {
            _velocity.Y += _player.jumpForce;
        }
    }

    public void OnFall(double delta)
    {
        if (_player.IsOnFloor())
        {
            _velocity.Y = 0;
        }
        else
        {
            _velocity.Y += _player.gravity * (float)delta;
        }
    }
}