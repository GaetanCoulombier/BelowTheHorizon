
using Godot;
using Godot.Collections;


public abstract partial class MovementState : Resource
{
    public float MovementSpeed { get; protected set; }
    public float Acceleration { get; protected set;}
    public float CameraFov { get; protected set;}
    public float AnimationSpeed { get; protected set;}
    public Dictionary<string, Vector3> InputActions { get; protected set; }
    public Vector3 PossibleDirections { get; protected set; }

    internal Dictionary<string, Vector3> GetInputActions()
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
    public static MovementState SWIM = new SwimState();
}

public enum MovementType
{
    NONE,
    GROUND,
    CLIMBING,
    SWIMMING,
}