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
    [Export] public float gravity = -9.81f;
    [Export] public float jumpForce = 5.0f;
    [Export] public float rotationSpeed = 8.0f;
    public MovementState movementState = MovementState.WALK;
    public MovementType movementType = MovementType.GROUND;

    /* Inputs */
    private Vector3 _inputDirection = Vector3.Zero;

    /* Signals */
    [Signal]
    public delegate void PlayerInputChangedEventHandler(Vector3 direction);
    [Signal]
    public delegate void PlayerInputChangedEventHandler(Vector3 direction);

    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("CamRoot/SpringArm3D/Camera3D");
        
        /* -- Initialize the controllers -- */
        var _cameraRoot = GetNode<Node3D>("CamRoot");
        var _meshRoot = GetNode<Node3D>("MeshRoot");
        _movementController = new MovementController(this, _meshRoot, _camera);
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
        _movementController.OnInputChanged(_inputDirection, delta);
        _movementController.Handle(delta);
    }


    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey && eventKey.Pressed)
        {
            // Réinitialiser la direction
            _inputDirection = Vector3.Zero;

            // Parcourir les actions configurées
            foreach (var (action, axis) in movementState.GetInputActions())
            {
                if (Input.IsActionPressed(action))
                {
                    _inputDirection += axis;
                }
            }

            // Normaliser pour éviter des mouvements plus rapides en diagonale
            _inputDirection = _inputDirection.Normalized();
        }
    }
}
