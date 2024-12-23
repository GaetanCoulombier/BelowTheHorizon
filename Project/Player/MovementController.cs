using System;
using Godot;

public partial class MovementController : Node
{
    /* Nodes */
    [Export] private PlayerController _player;
    [Export] private Node3D _meshRoot;
    [Export] private CameraController _camera;
    [Export] private RayCast3D _facingCheck;
    [Export] private RayCast3D _leftFacingCheck;
    [Export] private RayCast3D _rightFacingCheck;
    [Export] private RayCast3D _LeftSurfaceCheck;
    [Export] private RayCast3D _rightSurfaceCheck;

    /* Movement variables */
    private MovementType _movementType;
    private Vector3 _velocity = Vector3.Zero;
    private Vector3 _direction = Vector3.Zero;
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
        // Get the nodes
        _player = GetParent<PlayerController>();
        _meshRoot = _player.GetNode<Node3D>("MeshRoot");
        _camera = _player.GetNode<CameraController>("CameraController");

        // Climbing
        _facingCheck = _meshRoot.GetNode<RayCast3D>("DetectionController/Climbing/RayCastFacing");
        _leftFacingCheck = _meshRoot.GetNode<RayCast3D>("DetectionController/Climbing/RayCastLeftFacing");
        _rightFacingCheck = _meshRoot.GetNode<RayCast3D>("DetectionController/Climbing/RayCastRightFacing");
        _LeftSurfaceCheck = _meshRoot.GetNode<RayCast3D>("DetectionController/Climbing/RayCastLeftSurface");
        _rightSurfaceCheck = _meshRoot.GetNode<RayCast3D>("DetectionController/Climbing/RayCastRightSurface");

        // Get the initial rotation of the player
        _playerInitRotation = _player.Rotation.Y;

        // Connect the signals
        _player.Connect(nameof(PlayerController.ChangeMovementState), new Callable(this, nameof(OnChangeMovementState)));
        _player.Connect(nameof(PlayerController.ChangeInput), new Callable(this, nameof(OnChangeInput)));
        _player.Connect(nameof(PlayerController.Jump), new Callable(this, nameof(OnJump)));
        _player.Connect(nameof(PlayerController.Fall), new Callable(this, nameof(OnFall)));
        _camera.Connect(nameof(CameraController.ChangeCameraRotation), new Callable(this, nameof(OnCameraUpdate)));
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
        var rotatedDirection = _direction.Rotated(Vector3.Up, _cameraRotation);

        // Momentum if the player is in the air and not touching ground
        if (rotatedDirection == Vector3.Zero && !_player.IsOnFloor()) {
            _velocity.X *= _momentum;
            _velocity.Z *= _momentum;
            return;
        }

        // Apply the movement
        _velocity.X = rotatedDirection.X * _speed;
        _velocity.Z = rotatedDirection.Z * _speed;

        // Smoothly interpolate the current mesh rotation towards the target rotation
        if (rotatedDirection != Vector3.Zero)
        {
            float targetRotation = Mathf.Atan2(rotatedDirection.X, rotatedDirection.Z) - _playerInitRotation;
            float currentRotation = _meshRoot.Rotation.Y;
            _meshRoot.Rotation = new Vector3(
                _meshRoot.Rotation.X,
                Mathf.LerpAngle(currentRotation, targetRotation, (float) delta * _rotationSpeed),
                _meshRoot.Rotation.Z
            );
        }
    }

    private void UpdateClimbMovement(double delta)
    {
        // TODO : Monter en haut de la surface
        // TODO : Implementer le hanging
        // TODO : vérifier les murs adjacents pour pouvoir tourner si il en exisent
        // TODO : Régler le problème de poussé en diagonal lorsque le joueur monte ou descends une surface en dévers

        GD.Print("rightSurfaceCheck: " + _rightSurfaceCheck.IsColliding());
        GD.Print("leftSurfaceCheck: " + _LeftSurfaceCheck.IsColliding());

        // Check if the player is at the edge of the surface
        if (!_leftFacingCheck.IsColliding() && _direction.X > 0) _direction.X = 0;
        if (!_rightFacingCheck.IsColliding() && _direction.X < 0) _direction.X = 0;

        // Get the tangent of the surface
        var surfaceNormal = _facingCheck.GetCollisionNormal();
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

        // Rotate the player towards the surface
        _meshRoot.LookAt(_meshRoot.GlobalTransform.Origin + surfaceNormal, Vector3.Up);
        _meshRoot.Rotation = new Vector3(0, _meshRoot.Rotation.Y, 0);
    }



    private void UpdateSwimMovement(double delta)
    {
        throw new NotImplementedException();
    }



    /* Signals */
    public void OnChangeMovementState(MovementState state)
    {
        _speed = state.MovementSpeed;
        _acceleration = state.Acceleration;

        // Reset player rotation
    }
    
    public void OnChangeMovementType(MovementType type)
    {
        _movementType = type;
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

    public void OnCameraUpdate(float cameraRotation)
    {
        _cameraRotation = cameraRotation;
    }
}