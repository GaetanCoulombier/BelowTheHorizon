
public class DetectionController
{

    private PlayerController _playerController;

    public DetectionController(PlayerController playerController)
    {
        _playerController = playerController;
    }

    // For now, we only handle the ground detection
    public void Handle(double delta)
    {
        _playerController.movementType = MovementType.GROUND;
    }
}