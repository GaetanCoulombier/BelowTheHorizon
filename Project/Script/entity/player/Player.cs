using Godot;

public partial class Player : CharacterBody3D
{
    private PlayerMovementHandler _movementHandler;
    private PlayerClimbingHandler _climbingHandler;
    private InputHandler _inputHandler;
    private Camera3D _camera;

    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Camera");

        _movementHandler = new PlayerMovementHandler(this, _camera, 5.0f, -9.81f, 5.0f);
        _climbingHandler = new PlayerClimbingHandler(
            this,
            GetNode<RayCast3D>("ClimbingNode/RayCastFacingWall"),

            GetNode<RayCast3D>("ClimbingNode/FrontRight/RayCastUp"),
            GetNode<RayCast3D>("ClimbingNode/FrontRight/RayCastDown"),

            GetNode<RayCast3D>("ClimbingNode/FrontLeft/RayCastUp"),
            GetNode<RayCast3D>("ClimbingNode/FrontLeft/RayCastDown"),

            GetNode<RayCast3D>("ClimbingNode/FrontMiddle/RayCastUp"),
            GetNode<RayCast3D>("ClimbingNode/FrontMiddle/RayCastDown"),

            GetNode<RayCast3D>("ClimbingNode/Side/RayCastLeft"),
            GetNode<RayCast3D>("ClimbingNode/Side/RayCastRight")
        );

        _inputHandler = new InputHandler();
    }

    public override void _PhysicsProcess(double delta)
    {
        var movementInput = _inputHandler.GetMovementInput();
        var jumpPressed = _inputHandler.IsJumpPressed();

        _climbingHandler.AttachToLedge();

        _inputHandler.HandleClimbingInputs(_climbingHandler);
        if (!_climbingHandler.IsHanging)
        {
            _movementHandler.HandleMovement(movementInput, jumpPressed, delta);
        }
    }


}