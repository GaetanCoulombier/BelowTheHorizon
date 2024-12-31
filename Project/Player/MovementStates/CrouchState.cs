
public partial class CrouchState : MovementState
{
    public CrouchState() {
        MovementSpeed = 2.0f;
        Acceleration = 10.0f;
    }

    public override string ToString()
    {
        return "Crouch";
    }
}