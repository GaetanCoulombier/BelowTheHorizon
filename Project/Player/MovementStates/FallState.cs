
public partial class FallState : MovementState
{
    public FallState() {
        // Lower the speed and acceleration for air resistance
        MovementSpeed = 1.0f;
        Acceleration = 0.1f;
    }

    public override string ToString()
    {
        return "Idle";
    }
}