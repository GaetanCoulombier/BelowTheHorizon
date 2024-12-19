
public class CrouchState : GroundMovementState
{
    public CrouchState() {
        MovementSpeed = 4.0f;
        Acceleration = 10.0f;
        CameraFov = 60.0f;
        AnimationSpeed = 0.5f;
    }
}