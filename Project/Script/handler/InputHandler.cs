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
            if (IsCrouchPressed()) _climbingHandler.DropDown();
            else if (IsMoveLeftPressed()) _climbingHandler.MoveAlongLedge(-1);
            else if (IsMoveRightPressed()) _climbingHandler.MoveAlongLedge(1);
        } else {
            if (IsInteractPressed()) _climbingHandler.GrabLedge();
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
