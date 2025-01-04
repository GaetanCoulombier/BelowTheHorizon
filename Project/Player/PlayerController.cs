using Godot;

public partial class PlayerController : CharacterBody3D
{
    /* Inventory */
    [Export] public Inventory inventory;

    /* Movement configuration */
    public MovementState movementState { get; private set; }
    public MovementType movementType { get; private set; } = MovementType.GROUND;
    private Vector3 _lastInputDirection = Vector3.Zero;

    /* Signals */
    [Signal]
    public delegate void ChangeInputEventHandler(Vector3 direction);
    [Signal]
    public delegate void ChangeMovementStateEventHandler(MovementState state);
    [Signal]
    public delegate void JumpEventHandler();
    [Signal]
    public delegate void FallEventHandler();
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
                // Pre-calculations for better readability
                bool isOnFloor = IsOnFloor();
                bool isCrouching = Input.IsActionPressed("crouch");
                bool isSprinting = Input.IsActionPressed("sprint");
                bool isJumping = Input.IsActionJustPressed("jump");

                // Default state: IDLE or WALK
                if (_lastInputDirection == Vector3.Zero) // TODO verify with a raycast if the player is in front of a wall to set the state to IDLE
                    SetMovementState(MovementState.IDLE);
                else if (isCrouching)
                    SetMovementState(MovementState.CROUCH);
                else if (isSprinting)
                    SetMovementState(MovementState.RUN);
                else
                    SetMovementState(MovementState.WALK);

                if (isJumping && isOnFloor)
                    EmitSignal(nameof(Jump));
                else if (!isOnFloor)
                    EmitSignal(nameof(Fall));
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

        // Prevent from emitting the same signal
        if (inputDirection == _lastInputDirection) return;

        EmitSignal(nameof(ChangeInput), inputDirection);
        _lastInputDirection = inputDirection;
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

    public Inventory GetInventory()
    {
        return inventory;
    }
}