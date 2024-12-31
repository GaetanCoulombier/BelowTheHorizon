
public partial class SwimState : MovementState
{
    public SwimState() {
        MovementSpeed = 4.0f;
        Acceleration = 10.0f;
    }

    public override string ToString()
    {
        return "Swim";
    }
}