
public partial class GrovelState : MovementState
{
    public GrovelState() {
        speed = 2.0f;
        added_fov = 0.0f;
    }

    public override string ToString()
    {
        return "Grovel";
    }
}