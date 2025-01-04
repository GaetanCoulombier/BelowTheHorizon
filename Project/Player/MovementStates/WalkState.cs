
public partial class WalkState : MovementState
{
    public WalkState() {
        speed = 4.0f;
        added_fov = 0.0f;
    }

    public override string ToString()
    {
        return "Walk";
    }
}