
public partial class CrouchState : MovementState
{
    public CrouchState() {
        speed = 2.25f;
        addedFov = 0.0f;
        playerHeight = 1.0f;
    }

    public override string ToString()
    {
        return "Crouch";
    }
}