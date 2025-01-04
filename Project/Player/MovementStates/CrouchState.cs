
public partial class CrouchState : MovementState
{
    public CrouchState() {
        speed = 3.0f;
        added_fov = 0.0f;
    }

    public override string ToString()
    {
        return "Crouch";
    }
}