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
            GetNode<RayCast3D>("/root/GameRoot/Player/RayCastFacingWall"),
            GetNode<RayCast3D>("/root/GameRoot/Player/RaycastLedgeChecker/RayCastHead"),
            GetNode<Node3D>("/root/GameRoot/Player/RaycastLedgeChecker/RayCastHead/LedgeMarker"),
            GetNode<Node3D>("/root/GameRoot/Player/RaycastLedgeChecker")
        );

        _inputHandler = new InputHandler();
    }

    public override void _PhysicsProcess(double delta)
    {
        var movementInput = _inputHandler.GetMovementInput();
        var jumpPressed = _inputHandler.IsJumpPressed();

        _inputHandler.HandleClimbingInputs(_climbingHandler);
        if (!_climbingHandler.IsHanging)
        {
            _movementHandler.HandleMovement(movementInput, jumpPressed, delta);
        }

        _climbingHandler.DetectLedge();
    }


}