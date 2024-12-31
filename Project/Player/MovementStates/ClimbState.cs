
public partial class ClimbState : MovementState
{
    public ClimbState() {
        MovementSpeed = 4.0f;
        Acceleration = 10.0f;
    }

    public override string ToString()
    {
        return "Climb";
    }
}