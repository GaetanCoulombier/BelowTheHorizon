
public partial class ClimbState : MovementState
{
    public ClimbState() {
        speed = 2.0f;
        addedFov = 0.0f;
    }

    public override string ToString()
    {
        return "Climb";
    }
}