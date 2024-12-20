
using System.Collections.Generic;
using Godot;

public abstract class GroundMovementState : MovementState
{
    public GroundMovementState() {
        InputActions = new List<(string, Vector3)>
        {
            ("move_forward", Vector3.Forward),
            ("move_backward", Vector3.Back),
            ("move_left", Vector3.Left),
            ("move_right", Vector3.Right),
        };
        PossibleDirections = new Vector3(1, 0, 1);
    }
}