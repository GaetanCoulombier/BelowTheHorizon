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
    [Export] private RayCast3D _leftSurfaceCheck;
    [Export] private RayCast3D _rightSurfaceCheck;
    [Export] private RayCast3D _leftCheck;
    [Export] private RayCast3D _rightCheck;

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
    [Export] private float _jumpForce = 4.0f;
    [Export] private float _momentum = 0.98f;


    private AnimationTree _animationTree;



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
        _leftSurfaceCheck = _meshRoot.GetNode<RayCast3D>("DetectionController/Climbing/RayCastLeftSurface");
        _rightSurfaceCheck = _meshRoot.GetNode<RayCast3D>("DetectionController/Climbing/RayCastRightSurface");
        _leftCheck = _meshRoot.GetNode<RayCast3D>("DetectionController/Climbing/RayCastLeft");
        _rightCheck = _meshRoot.GetNode<RayCast3D>("DetectionController/Climbing/RayCastRight");

        // Get the initial rotation of the player
        _playerInitRotation = _player.Rotation.Y;

        // Connect the signals
        _player.Connect(nameof(PlayerController.ChangeMovementState), new Callable(this, nameof(OnChangeMovementState)));
        _player.Connect(nameof(PlayerController.ChangeInput), new Callable(this, nameof(OnChangeInput)));
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

        // Apply gravity
        if (!_player.IsOnFloor()) _velocity.Y += _gravity * (float) delta;
    }

    private void UpdateClimbMovement(double delta)
    {
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
    }
    
    public void OnChangeMovementType(MovementType type)
    {
        _movementType = type;
    }

    public void OnChangeInput(Vector3 direction)
    {
        _direction = direction;
    }

    public void OnCameraUpdate(float cameraRotation)
    {
        _cameraRotation = cameraRotation;
    }
}