
using System.Collections.Generic;
using Godot;

public abstract partial class ClimbMovementState : MovementState
{
    public ClimbMovementState() {
        InputActions = new List<(string, Vector3)>
        {
            ("move_left", Vector3.Left),
            ("move_right", Vector3.Right),
            ("move_up", Vector3.Up),
            ("move_down", Vector3.Down),
        };
        PossibleDirections = new Vector3(1, 1, 0);
    }
}