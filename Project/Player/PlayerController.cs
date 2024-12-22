using Godot;
using Godot.Collections;
using System;

public partial class PlayerController : CharacterBody3D
{
    /* Movement configuration */
    public MovementState movementState { get; private set; }
    public MovementType movementType { get; private set; } = MovementType.GROUND;
    private Vector3 _lastInputDirection = Vector3.Zero;

    /* Signals */
    [Signal]
    public delegate void ChangeInputEventHandler(Vector3 direction);
    [Signal]
    public delegate void JumpEventHandler();
    [Signal]
    public delegate void FallEventHandler();
    [Signal]
    public delegate void ChangeMovementStateEventHandler(MovementState state);
    [Signal]
    public delegate void ChangeMovementTypeEventHandler(MovementType type);



    /* Godot methods */
    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        UpdateInputs();
        UpdatePhysics();
    }



    /* Custom methods */
    private void UpdatePhysics()
    {
        switch (movementType)
        {
            case MovementType.GROUND:
                if (movementState == null) SetMovementState(MovementState.WALK); // Default state

                // Crouch
                if (Input.IsActionJustPressed("crouch")) SetMovementState(MovementState.CROUCH); // TODO : prevent from running will crouching
                if (Input.IsActionJustReleased("crouch")) SetMovementState(MovementState.WALK);

                // Sprint
                if (Input.IsActionPressed("sprint")) SetMovementState(MovementState.RUN); // TODO : prevent from running in the air
                if (Input.IsActionJustReleased("sprint")) SetMovementState(MovementState.WALK);

                // Jump and fall
                if (!IsOnFloor()) EmitSignal(nameof(Fall));
                if (Input.IsActionJustPressed("jump") && IsOnFloor()) EmitSignal(nameof(Jump));
                break;

            case MovementType.CLIMBING:
                if (movementState == null) SetMovementState(MovementState.CLIMB); // Default state
                break;

            case MovementType.SWIMMING:
                if (movementState == null) SetMovementState(MovementState.SWIM); // Default state
                break;

            default:
                GD.PrintErr("Movement state not implemented");
                break;
        }
    }

    private void UpdateInputs()
    {
        var inputDirection = Vector3.Zero;

        switch (movementType)
        {
            case MovementType.GROUND:
                if (Input.IsActionPressed("move_left")) inputDirection += Vector3.Left;
                if (Input.IsActionPressed("move_right")) inputDirection += Vector3.Right;
                if (Input.IsActionPressed("move_forward")) inputDirection += Vector3.Forward;
                if (Input.IsActionPressed("move_backward")) inputDirection += Vector3.Back;
                break;

            case MovementType.CLIMBING:
                if (Input.IsActionPressed("move_left")) inputDirection += Vector3.Right;
                if (Input.IsActionPressed("move_right")) inputDirection += Vector3.Left;
                if (Input.IsActionPressed("move_forward")) inputDirection += Vector3.Down;
                if (Input.IsActionPressed("move_backward")) inputDirection += Vector3.Up;
                break;

            case MovementType.SWIMMING:
                if (Input.IsActionPressed("move_left")) inputDirection += Vector3.Left;
                if (Input.IsActionPressed("move_right")) inputDirection += Vector3.Right;
                if (Input.IsActionPressed("move_forward")) inputDirection += Vector3.Forward;
                if (Input.IsActionPressed("move_backward")) inputDirection += Vector3.Back;
                if (Input.IsActionPressed("move_up")) inputDirection += Vector3.Up;
                if (Input.IsActionPressed("move_down")) inputDirection += Vector3.Down;
                break;

            default:
                GD.PrintErr("Movement state not implemented");
                break;
        }

        // Normalize to prevent diagonal movement to be faster
        inputDirection = inputDirection.Normalized();

        if (inputDirection != _lastInputDirection)
        {
            EmitSignal(nameof(ChangeInput), inputDirection);
            _lastInputDirection = inputDirection;
        }
    }

    public void SetMovementState(MovementState state)
    {
        if (movementState == state) return;
        movementState = state;
        EmitSignal(nameof(ChangeMovementState), state);
    }

    public void SetMovementType(MovementType type)
    {
        if (movementType == type) return;
        movementState = null; // reset the movement state
        movementType = type;
        EmitSignal(nameof(ChangeMovementType));
    }
}
