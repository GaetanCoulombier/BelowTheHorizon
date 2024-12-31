
public partial class RunState : MovementState
{
    public RunState() {
        MovementSpeed = 5.0f;
        Acceleration = 30.0f;
    }

    public override string ToString()
    {
        return "Run";
    }
}