using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
    /* Nodes */
    private Camera3D _camera;

    /* Controllers */
    private DetectionController _detectionController; // Using raycasts to detect ground, walls, etc.
    private MovementController _movementController; // Handling movement and physics

    /* Movement configuration */
    public MovementState movementState { get; private set; } = MovementState.WALK;
    public MovementType movementType { get; private set; } = MovementType.GROUND;

    /* Signals */
    [Signal] public delegate void SetMovementStateEventHandler();
    [Signal] public delegate void SetMovementTypeEventHandler();


    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("CamRoot/SpringArm3D/Camera3D");

        _movementController = new MovementController(this, _camera);
        _detectionController = new DetectionController(this);
    }

    public override void _PhysicsProcess(double delta)
    {
        /* -- Test the player movement type -- */
        _detectionController.Handle(delta);

        /* -- Handle the player input for all possible movement types -- */
        if (movementType == MovementType.GROUND){
            // Crouch
            if (Input.IsActionJustPressed("crouch")) movementState = MovementState.CROUCH;
            if (Input.IsActionJustReleased("crouch")) movementState = MovementState.WALK;

            // Sprint
            if (Input.IsActionPressed("sprint")) movementState = MovementState.RUN;
            if (Input.IsActionJustReleased("sprint")) movementState = MovementState.WALK;

            // Gravity
            _movementController.OnFall(delta);

            // Movements
            if (Input.IsActionJustPressed("jump")) _movementController.OnJump();
            _movementController.OnInputChanged(CalculateMoveDirectionFromInputs());
        }
        else if (movementType == MovementType.CLIMBING)
        {
            // Handle climbing input
        }
        else if (movementType == MovementType.SWIMMING)
        {
            movementState = MovementState.SWIM;
            // Handle swimming input
        }


        /* -- Apply movement -- */
        _movementController.OnInputChanged(CalculateMoveDirectionFromInputs());
        _movementController.Handle(delta, CalculateMoveDirectionFromInputs());
    }

    /*
     * Calculate the movement direction based on the inputs and the camera orientation
     * 
     * TODO : Add an _Input() method to handle the inputs for better performances
     */
    public virtual Vector3 CalculateMoveDirectionFromInputs()
    {
        Vector3 movement = Vector3.Zero;

        foreach (var (action, axis) in movementState.GetInputActions())
        {
            if (Input.IsActionPressed(action))
            {
                movement += axis;
            }
        }

        return movement.Normalized();
    }

    public void SetMovementType(MovementType type)
    {
        movementType = type;
        EmitSignal(nameof(SetMovementTypeHandler));
    }

    public void SetMovementState(MovementState state)
    {
        movementState = state;
        EmitSignal(nameof(SetMovementStateEventHandler));
    }
}
