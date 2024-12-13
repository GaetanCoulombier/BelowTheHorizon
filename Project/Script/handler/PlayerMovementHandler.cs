using Godot;

public class PlayerMovementHandler
{
    private CharacterBody3D _player;
    private Camera3D _camera;
    private float _speed;
    private float _gravity;
    private float _jumpStrength;
    private Vector3 _velocity;

    public PlayerMovementHandler(CharacterBody3D player, Camera3D camera, float speed, float gravity, float jumpStrength)
    {
        _player = player;
        _camera = camera;
        _speed = speed;
        _gravity = gravity;
        _jumpStrength = jumpStrength;
        _velocity = Vector3.Zero;
    }

    public void HandleMovement(Vector3 input, bool jump, double delta)
    {
        AlignPlayerToCamera();
        ApplyGravity(delta);

        Vector3 horizontalMovement = new Vector3(input.X, 0, input.Z) * _speed;
        _velocity.X = TransformDirection(horizontalMovement).X;
        _velocity.Z = TransformDirection(horizontalMovement).Z;

        if (jump && _player.IsOnFloor())
        {
            _velocity.Y = _jumpStrength;
        }

        _player.Velocity = _velocity;
        _player.MoveAndSlide();
    }

    private void AlignPlayerToCamera()
    {
        Vector3 directionToCamera = _camera.GlobalPosition - _player.GlobalTransform.Origin;
        directionToCamera.Y = 0;
        _player.LookAt(_player.GlobalTransform.Origin - directionToCamera.Normalized(), Vector3.Up);
    }

    private void ApplyGravity(double delta)
    {
        if (!_player.IsOnFloor()) _velocity.Y += _gravity * (float)delta;
        else if (_velocity.Y < 0) _velocity.Y = 0;
    }

    private Vector3 TransformDirection(Vector3 localDirection)
    {
        Basis cameraBasis = _camera.GlobalTransform.Basis;
        return new Basis(cameraBasis.X, Vector3.Up, cameraBasis.Z) * localDirection;
    }
}
