
using Godot;

public abstract partial class MovementState : Resource
{
    public float speed { get; protected set; }
    public float addedFov { get; protected set; }
    public float playerHeight { get; protected set; } = 1.7f;

    // Ground
    public static MovementState IDLE = new IdleState();
    public static MovementState WALK = new WalkState();
    public static MovementState RUN = new RunState();
    public static MovementState CROUCH = new CrouchState();
    public static MovementState CRAWL = new CrawlState();

    // Climbing
    public static MovementState CLIMB = new ClimbState();
    //public static MovementState HANG = new HangState();

    // Swimming
    public static MovementState SWIM = new SwimState();


    public bool IsOneOf(params MovementState[] states)
    {
        foreach (var state in states)
        {
            if (this == state) return true;
        }
        return false;
    }

}

public enum MovementType
{
    NONE,
    GROUND,
    CLIMBING,
    SWIMMING,
}