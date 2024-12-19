
using System.Collections.Generic;
using Godot;

public abstract class MovementState
{
    public int Id { get; private set; }
    public float MovementSpeed { get; protected set; }
    public float Acceleration { get; protected set;}
    public float CameraFov { get; protected set;}
    public float AnimationSpeed { get; protected set;}
    public List<(string, Vector3)> InputActions { get; protected set; }
    public Vector3 PossibleDirections { get; protected set; }

    public MovementState()
    {
        Id = GetHashCode();
    }

    public MovementState GetFromId(int id)
    {
        if (id == WALK.Id) return WALK;
        else if (id == RUN.Id) return RUN;
        else if (id == CROUCH.Id) return CROUCH;
        else return null;

    }

    internal List<(string, Vector3)> GetInputActions()
    {
        return InputActions;
    }

    // Ground
    public static MovementState WALK = new WalkState();
    public static MovementState RUN = new RunState();
    public static MovementState CROUCH = new CrouchState();

    // Climbing
    //public static MovementState CLIMB = new ClimbState();
    //public static MovementState HANG = new HangState();

    // Swimming
    //public static MovementState SWIM = new SwimState();
}

public enum MovementType
{
    GROUND,
    CLIMBING,
    SWIMMING,
}