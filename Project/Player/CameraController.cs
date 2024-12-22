using Godot;

public partial class CameraController : Node3D
{
	/* Componants */
	[Export] private Camera3D _camera;
	[Export] private Node3D _pivot;
	[Export] private Node3D _player;
	[Export] private Node3D _yawNode;
	[Export] private Node3D _pitchNode;

	/* Variables */
	private Tween tween;
	private float yaw = 0;
	private float pitch = 0;

	/* Settings */
    private float _sensitivity = 0.1f; // TODO : Add this to the settings menu
    private float _maxVerticalAngle = 90; // TODO : Add this to the settings menu

	/* Signals */
	[Signal]
	public delegate void ChangeCameraRotationEventHandler(float cameraRotation);

	public override void _Ready()
	{
		_yawNode = GetNode<Node3D>("Yaw");
		_pitchNode = _yawNode.GetNode<Node3D>("Pitch");
		_camera = _pitchNode.GetNode<Camera3D>("SpringArm3D/Camera3D");
		_player = GetParent<Node3D>();

		// Hide the mouse cursor
		Input.SetMouseMode(Input.MouseModeEnum.Captured);

		// Signal
        _player.Connect(nameof(PlayerController.ChangeMovementState), new Callable(this, nameof(OnChangeMovementState)));
	}

	public override void _PhysicsProcess(double delta)
	{
		pitch = Mathf.Clamp(pitch, -_maxVerticalAngle, _maxVerticalAngle);

		_yawNode.RotationDegrees = new Vector3(0, yaw, 0);
		_pitchNode.RotationDegrees = new Vector3(pitch, 0, 0);

		EmitSignal(nameof(ChangeCameraRotation), _yawNode.Rotation.Y);
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion mouseEvent) {
			yaw -= mouseEvent.Relative.X * _sensitivity;
			pitch -= mouseEvent.Relative.Y * _sensitivity;
		}
	}

	/* Signals */
	private void OnChangeMovementState(MovementState state)
	{
		tween?.Kill();

		tween = CreateTween();
		tween.TweenProperty(_camera, "fov", state.CameraFov, 0.3f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
	}
}