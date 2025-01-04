
using Godot;

public abstract partial class MovementState : Resource
{
    public float speed { get; protected set; }
    public float added_fov { get; protected set; }

    // Ground
    public static MovementState IDLE = new IdleState();
    public static MovementState WALK = new WalkState();
    public static MovementState RUN = new RunState();
    public static MovementState CROUCH = new CrouchState();

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