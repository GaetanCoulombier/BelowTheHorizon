
public partial class SwimState : MovementState
{
    public SwimState() {
        speed = 4.0f;
        added_fov = 0.0f;
    }

    public override string ToString()
    {
        return "Swim";
    }
}