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
	private bool _isPaused = false;
	private Tween tween;
	private float yaw = 0;
	private float pitch = 0;

	/* Settings */
	private float _fov = 45;
    private float _sensitivity = 0.1f; // TODO : Add this to the settings menu
    private float _maxVerticalAngle = 90; // TODO : Add this to the settings menu

	/* Signals */
	[Signal]
	public delegate void ChangeCameraRotationEventHandler(float cameraRotation);



	/* Godot methods */
	public override void _Ready()
	{
		_yawNode = GetNode<Node3D>("Yaw");
		_pitchNode = _yawNode.GetNode<Node3D>("Pitch");
		_camera = _pitchNode.GetNode<Camera3D>("SpringArm3D/Camera3D");
		_player = GetParent<Node3D>();

		// Set the camera settings
		_camera.Fov = _fov;

		// Hide the mouse cursor
		Input.SetMouseMode(Input.MouseModeEnum.Captured);

		// Signal
		GetNode<GameController>("/root/GameRoot/GameController").Connect(nameof(GameController.TriggerPause), new Callable(this, nameof(OnTriggerPause)));
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_isPaused) return;

		pitch = Mathf.Clamp(pitch, -_maxVerticalAngle, _maxVerticalAngle);

		_yawNode.RotationDegrees = new Vector3(0, yaw, 0);
		_pitchNode.RotationDegrees = new Vector3(pitch, 0, 0);

		EmitSignal(nameof(ChangeCameraRotation), _yawNode.Rotation.Y);
	}

	public override void _Input(InputEvent @event)
	{
		if (_isPaused) return;
		
		if (@event is InputEventMouseMotion mouseEvent) {
			yaw -= mouseEvent.Relative.X * _sensitivity;
			pitch -= mouseEvent.Relative.Y * _sensitivity;
		}
	}



	/* Signals */
	private void OnTriggerPause(bool isPaused)
	{
		Input.SetMouseMode(isPaused ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured);
		_isPaused = isPaused;
	}
}