
public partial class RunState : MovementState
{
    public RunState() {
        speed = 6.0f;
        addedFov = 5.0f;
    }

    public override string ToString()
    {
        return "Run";
    }
}