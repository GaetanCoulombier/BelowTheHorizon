using System;
using Godot;

public partial class CameraController : Camera3D
{
	/* Componants */
	[Export] private PlayerController _player;
	[Export] private MovementController _movementController;
	[Export] private Node3D _head;
	private Tween tween;

	/* Settings */
    private const float SENSITIVITY = 0.1f; // TODO : Add this to the settings menu
	private const float MAX_ANGLE = 89;
	private float BASE_FOV = 60;

	/* Head bobbing */
	private const float BOB_AMP = 0.1f;
	private const float BOB_FREQ = 2.0f;
	private float _bobTime = 0.0f;

	/* Godot methods */
	public override void _Ready()
	{
		// Set the camera settings
		this.Fov = BASE_FOV;

		// Hide the mouse cursor
		Input.SetMouseMode(Input.MouseModeEnum.Captured);

        // Signal
		_movementController.Connect(nameof(MovementController.ChangeMovementState), new Callable(this, nameof(OnChangeMovementState)));
	}

	public override void _Process(double delta)
	{
		if (GameState.isActionsBlocked) return;

        // Bobbing effect
		_bobTime += (float) delta * _player.Velocity.Length() * (_player.IsOnFloor() ? 1.0f : 0.0f);
		var transform = Transform;
		transform.Origin = HeadBobbing(_bobTime);
		Transform = transform;
	}

    public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseEvent) {
			if (GameState.isActionsBlocked) return;

			// Rotate the player
			_head.RotateY(Mathf.DegToRad(-mouseEvent.Relative.X * SENSITIVITY));
			this.RotateX(Mathf.DegToRad(-mouseEvent.Relative.Y * SENSITIVITY));
			this.Rotation = new Vector3(Mathf.Clamp(this.Rotation.X, Mathf.DegToRad(-MAX_ANGLE), Mathf.DegToRad(MAX_ANGLE)), this.Rotation.Y, this.Rotation.Z);
		}
	}

    private Vector3 HeadBobbing(float time)
    {
		var pos = Vector3.Zero;
		pos.Y = Mathf.Sin(time * BOB_FREQ) * BOB_AMP;
		pos.X = MathF.Cos(time * BOB_FREQ / 2) *BOB_AMP;
		return pos;
    }

	/* Signals */
	public void OnChangeMovementState(MovementState movementState)
    {
		tween?.Kill();

		tween = CreateTween();
		var newFov = movementState.added_fov + BASE_FOV;
		tween.TweenProperty(this, "fov", newFov, 0.3f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);
    }
}