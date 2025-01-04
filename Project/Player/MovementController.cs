using System;
using Godot;

public partial class MovementController : Node
{
    /* --- Nodes --- */
    [Export] private PlayerController _player;
    [Export] private Node3D _head;

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
    private Vector3 _lastInputDirection = Vector3.Zero;
    private float _speed = 0.0f;

    /* --- Settings --- */
    [Export] private float Gravity = -9.8f;
    [Export] private float JumpForce = 3.0f;
    [Export] private float Momentum = 0.98f;

    /* --- Godot Methods --- */
    public override void _Ready()
    {
        // Initial setup
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateMovementState();
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

        if (tempDirection != _lastInputDirection)
        {
            _direction = tempDirection;
            _lastInputDirection = tempDirection;
        }
    }

    private void UpdateMovementState()
    {
        switch (MovementType)
        {
            case MovementType.GROUND:
                UpdateGroundMovementState();
                UpdateGroundMovement(GetProcessDeltaTime());
                break;

            case MovementType.SWIMMING:
                if (MovementState == null) SetMovementState(MovementState.SWIM);
                UpdateSwimMovement(GetProcessDeltaTime());
                break;

            default:
                GD.PrintErr("Movement state not implemented");
                break;
        }
    }

    private void UpdateGroundMovementState()
    {
        bool isOnFloor = _player.IsOnFloor();
        bool isCrouching = Input.IsActionPressed("crouch");
        bool isSprinting = Input.IsActionPressed("sprint");
        bool isJumping = Input.IsActionJustPressed("jump");

        // Update movement state based on input conditions
        if (_lastInputDirection == Vector3.Zero)
        {
            SetMovementState(MovementState.IDLE);
        }
        else if (isCrouching)
        {
            SetMovementState(MovementState.CROUCH);
        }
        else if (isSprinting)
        {
            SetMovementState(MovementState.RUN);
        }
        else
        {
            SetMovementState(MovementState.WALK);
        }

        // Handle jump and fall
        if (isJumping && isOnFloor)
        {
            _velocity.Y = JumpForce;  // Apply jump force directly to velocity
            EmitSignal(nameof(Jump));  // Emit the Jump signal
        }
        else if (!isOnFloor)
        {
            _velocity.Y += Gravity * (float)GetProcessDeltaTime();  // Apply gravity when in the air
            EmitSignal(nameof(Fall));  // Emit the Fall signal
        }

        // Call movement update
        UpdateGroundMovement(GetProcessDeltaTime());
    }

    private void UpdateGroundMovement(double delta)
    {
        var rotatedDirection = _direction.Normalized().Rotated(Vector3.Up, _head.Rotation.Y);

        if (_player.IsOnFloor())
        {
            _velocity.X = rotatedDirection.X * _speed;
            _velocity.Z = rotatedDirection.Z * _speed;
        }
        else
        {
            _velocity.X = Mathf.Lerp(_velocity.X, rotatedDirection.X * _speed, (float)delta * Momentum);
            _velocity.Z = Mathf.Lerp(_velocity.Z, rotatedDirection.Z * _speed, (float)delta * Momentum);
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

    /* --- Getters & Setters --- */
    public void SetMovementState(MovementState state)
    {
        if (MovementState == state) return;
        MovementState = state;
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

    /* --- Signal Handlers --- */
    public void OnChangeMovementState(MovementState movementState)
    {
        _speed = movementState.speed;
    }

    public void OnJump()
    {
        _velocity.Y = JumpForce;  // This is now handled directly in UpdateGroundMovementState
    }

    public void OnFall()
    {
        _velocity.Y += Gravity * (float)GetProcessDeltaTime();  // This is now handled directly in UpdateGroundMovementState
    }
}
