
public partial class IdleState : MovementState
{
    public IdleState() {
        speed = 0.0f;
        addedFov = 0.0f;
    }

    public override string ToString()
    {
        return "Idle";
    }
}