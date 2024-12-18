using Godot;

public class InputHandler
{
    public Vector3 GetMovementInput()
    {
        Vector3 inputVector = Vector3.Zero;
        if (IsMoveForwardPressed()) inputVector.Z -= 1;
        if (IsMoveBackwardPressed()) inputVector.Z += 1;
        if (IsMoveLeftPressed()) inputVector.X -= 1;
        if (IsMoveRightPressed()) inputVector.X += 1;

        return inputVector.Normalized();
    }

    public void HandleClimbingInputs(PlayerClimbingHandler _climbingHandler)
    {
        //if (IsClimbUpPressed()) _climbingHandler.ClimbUp();
        if (_climbingHandler.IsHanging) {
            if (IsMoveBackwardPressed()) _climbingHandler.DropDown();
            else if (IsMoveForwardPressed()) _climbingHandler.ClimbUp();
            else if (IsMoveLeftPressed()) _climbingHandler.MoveAlongLedge((int) ClimbingDirection.Left);
            else if (IsMoveRightPressed()) _climbingHandler.MoveAlongLedge((int) ClimbingDirection.Right);
        }
    }


    /* All movements inputs possible for the player */
    public bool IsMoveForwardPressed() => Input.IsActionPressed("move_forward");
    public bool IsMoveBackwardPressed() => Input.IsActionPressed("move_backward");
    public bool IsMoveLeftPressed() => Input.IsActionPressed("move_left");
    public bool IsMoveRightPressed() => Input.IsActionPressed("move_right");
    public bool IsJumpPressed() => Input.IsActionJustPressed("jump");
    public bool IsCrouchPressed() => Input.IsActionJustPressed("crouch");
    public bool IsInteractPressed() => Input.IsActionJustPressed("interact");
}
