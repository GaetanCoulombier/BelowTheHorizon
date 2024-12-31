
public partial class IdleState : MovementState
{
    public IdleState() {
        // Make sure to set a value different from 0.0f to avoid the player to be stuck
        MovementSpeed = 0.1f;
        Acceleration = 0.1f;
    }

    public override string ToString()
    {
        return "Idle";
    }
}