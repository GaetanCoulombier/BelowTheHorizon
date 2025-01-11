
public partial class SwimState : MovementState
{
    public SwimState() {
        speed = 4.0f;
        addedFov = 0.0f;
    }

    public override string ToString()
    {
        return "Swim";
    }
}