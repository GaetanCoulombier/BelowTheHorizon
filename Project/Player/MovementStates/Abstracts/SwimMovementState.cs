using Godot;
using Godot.Collections;

public abstract partial class SwimMovementState : MovementState
{
    public SwimMovementState() {
        InputActions = new Dictionary<string, Vector3>{
            {"move_forward", Vector3.Forward},
            {"move_backward", Vector3.Back},
            {"move_left", Vector3.Left},
            {"move_right", Vector3.Right},
            {"move_up", Vector3.Up},
            {"move_down", Vector3.Down},
        };
        PossibleDirections = new Vector3(1, 1, 1);
    }
}