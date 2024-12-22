using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
    /* Controllers */
    private DetectionController _detectionController; // Using raycasts to detect ground, walls, etc.

    /* Movement configuration */
    public MovementState movementState { get; private set; } = MovementState.WALK;
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


    public override void _Ready()
    {        
        /* -- Initialize the controllers -- */
        _detectionController = new DetectionController(this);
    }

    public override void _Process(double delta)
    {
        UpdateInputs();
        UpdatePhysics();
    }

    private void UpdatePhysics()
    {
        // TODO : Godot process
        //_detectionController.Handle(delta);

        /* -- Handle the player input for all possible movement types -- */
        if (movementType == MovementType.GROUND){
            // Walk
            SetMovementState(MovementState.WALK);

            // Crouch
            if (Input.IsActionJustPressed("crouch")) SetMovementState(MovementState.CROUCH);
            if (Input.IsActionJustReleased("crouch")) SetMovementState(MovementState.WALK);

            // Sprint
            if (Input.IsActionPressed("sprint")) SetMovementState(MovementState.RUN);
            if (Input.IsActionJustReleased("sprint")) SetMovementState(MovementState.WALK);

            // Jump and fall
            if (!IsOnFloor()) EmitSignal(nameof(Fall));
            if (Input.IsActionJustPressed("jump") && IsOnFloor()) EmitSignal(nameof(Jump));
        }
        else if (movementType == MovementType.CLIMBING)
        {
            // Handle climbing input
        }
        else if (movementType == MovementType.SWIMMING)
        {
            SetMovementState(MovementState.SWIM);
            // Handle swimming input
        }
    }

    private void UpdateInputs()
    {
        // Réinitialiser la direction
        var inputDirection = Vector3.Zero;

        // Parcourir les actions configurées
        foreach (var (action, axis) in movementState.GetInputActions())
        {
            if (Input.IsActionPressed(action))
            {
                inputDirection += axis;
            }
        }

        // Normaliser pour éviter des mouvements plus rapides en diagonale
        inputDirection = inputDirection.Normalized();

        // Si la direction a changé, émettre le signal
        if (inputDirection != _lastInputDirection)
        {
            EmitSignal(nameof(ChangeInput), inputDirection);
            _lastInputDirection = inputDirection;
            GD.Print(inputDirection);
        }
    }

    public void SetMovementState(MovementState state)
    {
        movementState = state;
        EmitSignal(nameof(ChangeMovementState), state);
    }

    public void SetMovementType(MovementType type)
    {
        movementType = type;
        EmitSignal(nameof(ChangeMovementType));
    }
}
