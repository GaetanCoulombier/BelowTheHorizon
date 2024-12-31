
public partial class WalkState : MovementState
{
    public WalkState() {
        MovementSpeed = 3.0f;
        Acceleration = 15.0f;
    }

    public override string ToString()
    {
        return "Walk";
    }
}