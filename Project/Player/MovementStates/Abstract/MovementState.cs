
using Godot;

public abstract partial class MovementState : Resource
{
    public float MovementSpeed { get; protected set; }
    public float Acceleration { get; protected set;}

    // Ground
    public static MovementState IDLE = new IdleState();
    public static MovementState WALK = new WalkState();
    public static MovementState RUN = new RunState();
    public static MovementState CROUCH = new CrouchState();
    public static MovementState FALL = new FallState();

    // Climbing
    public static MovementState CLIMB = new ClimbState();
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