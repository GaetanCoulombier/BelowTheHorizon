
public partial class WalkState : MovementState
{
    public WalkState() {
        MovementSpeed = 6.0f;
        Acceleration = 15.0f;
        CameraFov = 75.0f;
        AnimationSpeed = 1.0f;
    }
}