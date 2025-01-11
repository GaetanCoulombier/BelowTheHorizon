using System;
using Godot;

public partial class MovementController : Node
{
    /* --- Nodes --- */
    [Export] private PlayerController _player;
    [Export] private Node3D _head;
    [Export] private CollisionShape3D _hitbox;
    [Export] private RayCast3D _headBonker;

    /* --- Signals --- */
    [Signal] public delegate void ChangeMovementStateEventHandler(MovementState state);
    [Signal] public delegate void ChangeMovementTypeEventHandler(MovementType type);
    [Signal] public delegate void JumpEventHandler();
    [Signal] public delegate void FallEventHandler();

    /* --- Properties --- */
    public MovementState MovementState { get; private set; }
    public MovementType MovementType { get; private set; } = MovementType.GROUND;

    /* --- Movement Variables --- */
    private Vector3 _velocity = Vector3.Zero;
    private Vector3 _direction = Vector3.Zero;
    private float _speed = 0.0f;
    private float _currentHeight = 2.0f;
    private float _targetHeight;
    private bool _isCrouching = false;
    private bool _isCrawling = false;


    /* --- Settings --- */
    [Export] private float Gravity = -9.8f;
    [Export] private float JumpForce = 4.0f;
    [Export] private float AirMomentum = 0.98f;
    [Export] private float GroundMomentum = 10.0f;
    [Export] private float SwimMomentum; // TODO : Add the swimming

    /* --- Godot Methods --- */
    public override void _Ready()
    {
        // Initial setup
    }

    public override void _PhysicsProcess(double delta)
    {
        // Update the player's height for the current movement state (Crouch, Crawl, Swim, etc.)
        AdaptPlayerHeight();
        
        // Update the movement based on the current state
        UpdateMovement(delta);

        // Apply the velocity to the player and move it
        _player.SetVelocity(_velocity);
        _player.MoveAndSlide();
    }

    public override void _Input(InputEvent @event)
    {
        HandleInput();
    }

    /* --- Custom Methods --- */
    private void HandleInput()
    {
        var tempDirection = Vector3.Zero;

        if (Input.IsActionPressed("move_left")) tempDirection += Vector3.Left;
        if (Input.IsActionPressed("move_right")) tempDirection += Vector3.Right;
        if (Input.IsActionPressed("move_forward")) tempDirection += Vector3.Forward;
        if (Input.IsActionPressed("move_backward")) tempDirection += Vector3.Back;

        _direction = tempDirection;
    }

    private void UpdateMovement(double delta)
    {
        switch (MovementType)
        {
            case MovementType.GROUND:
                UpdateGroundMovement(delta);
                break;

            case MovementType.CLIMBING:
                UpdateClimbMovement(delta);
                break;

            case MovementType.SWIMMING:
                if (MovementState == null) SetMovementState(MovementState.SWIM);
                UpdateSwimMovement(delta);
                break;

            default:
                GD.PrintErr("Movement state not implemented");
                break;
        }
    }

    private void UpdateGroundMovement(double delta)
    {
        bool isOnFloor = _player.IsOnFloor();

        bool isSprinting = Input.IsActionPressed("sprint");
        bool isJumping = Input.IsActionJustPressed("jump");

        bool isCrouchingPressed = Input.IsActionJustPressed("crouch"); // Use IsActionJustPressed for toggle
        bool isCrawlingPressed = Input.IsActionJustPressed("crawl");  // Use IsActionJustPressed for toggle

        // Toggle crouch state
        if (isCrouchingPressed) 
        {
            _isCrouching = !_isCrouching;
            _isCrawling = false; // Disable crawling if toggling crouch
        }

        // Toggle crawl state
        if (isCrawlingPressed) 
        {
            _isCrawling = !_isCrawling;
            _isCrouching = false; // Disable crouching if toggling crawl
        }

        // Update movement state based on toggles
        if (_isCrawling)
        {
            SetMovementState(MovementState.CRAWL);
        }
        else if (_isCrouching)
        {
            SetMovementState(MovementState.CROUCH);
        }
        else if (_direction == Vector3.Zero)
        {
            SetMovementState(MovementState.IDLE);
        }
        else if (isSprinting)
        {
            SetMovementState(MovementState.RUN);
        }
        else
        {
            SetMovementState(MovementState.WALK);
        }

        // Handle jumping
        if (isJumping && isOnFloor) { 
            _velocity.Y = JumpForce;
            EmitSignal(nameof(Jump));
        }

        // Handle gravity
        if (!isOnFloor) {
            _velocity.Y = Mathf.Lerp(_velocity.Y, Gravity, (float)delta);
            EmitSignal(nameof(Fall));
        }

        // If the player is colliding with something above, make it bounce
        if (_headBonker.IsColliding()) 
            _velocity.Y = -2;

        // Update the movement based on the current state
        var rotatedDirection = _direction.Normalized().Rotated(Vector3.Up, _head.Rotation.Y);

        if (_player.IsOnFloor())
        {
            if (_direction != Vector3.Zero)
            {
                // Acceleration based on the direction
                _velocity.X = Mathf.Lerp(_velocity.X, rotatedDirection.X * _speed, (float)delta * GroundMomentum);
                _velocity.Z = Mathf.Lerp(_velocity.Z, rotatedDirection.Z * _speed, (float)delta * GroundMomentum);
            }
            else
            {
                // Deceleration when no input
                _velocity.X = Mathf.Lerp(_velocity.X, 0, (float)delta * GroundMomentum);
                _velocity.Z = Mathf.Lerp(_velocity.Z, 0, (float)delta * GroundMomentum);
            }
        }
        else
        {
            // Air control
            _velocity.X = Mathf.Lerp(_velocity.X, rotatedDirection.X * _speed, (float)delta * AirMomentum);
            _velocity.Z = Mathf.Lerp(_velocity.Z, rotatedDirection.Z * _speed, (float)delta * AirMomentum);
        }
    }


    private void UpdateSwimMovement(double delta)
    {
        throw new NotImplementedException();
    }

    private void UpdateClimbMovement(double delta)
    {
        throw new NotImplementedException();
    }




    /* --- Movement Methods --- */
    public void AdaptPlayerHeight()
    {
        if (_targetHeight == _currentHeight) return;

        // Si ça touche, et que c'est en tain de monter, alors on ne change pas la hauteur
        if (_headBonker.IsColliding() && _targetHeight > _currentHeight) return;

        // Smoothly interpolate the height
        var currentShape = (CapsuleShape3D)_hitbox.Shape;
        _currentHeight = Mathf.Lerp(_currentHeight, _targetHeight, (float)(GetProcessDeltaTime() * 10)); // Adjust the speed factor
        currentShape.Height = _currentHeight;
    }





    /* --- Getters & Setters --- */
    public void SetMovementState(MovementState state)
    {
        // The player can't change movement state if it's colliding with something above
        if (_headBonker.IsColliding() && state.playerHeight > _currentHeight) return;

        MovementState = state;
        _targetHeight = state.playerHeight;
        _speed = state.speed;  // Update speed directly in the setter


        EmitSignal(nameof(ChangeMovementState), state);  // Emit the ChangeMovementState signal
    }

    public void SetMovementType(MovementType type)
    {
        if (MovementType == type) return;
        MovementState = null;  // Reset movement state on type change
        MovementType = type;
        EmitSignal(nameof(ChangeMovementType));  // Emit the ChangeMovementType signal
    }
}
