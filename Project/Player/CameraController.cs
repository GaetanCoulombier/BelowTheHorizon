using Godot;
using System;

public partial class CameraController : Node3D
{
	/* Componants */
	private Node3D _yawNode;
	private Node3D _pitchNode;
	private Camera3D _camera;

	/* Variables */
	private float _yaw = 0;
	private float _pitch = 0;
	private float _sensitivity = 0.07f;
	private float _acceleration = 15f;
	private float _maxPitch = 90f;

	public override void _Ready()
	{
		_yawNode = GetNode<Node3D>("CamYaw");
		_pitchNode = GetNode<Node3D>("CamYaw/CamPitch");
		_camera = GetNode<Camera3D>("CamYaw/CamPitch/SpringArm3D/Camera3D");
	}

	public override void _Process(double delta)
	{
		_pitch = Mathf.Clamp(_pitch, -_maxPitch, _maxPitch);

		// Lerp for smooth camera movement otherwise you can just set the rotation directly
		var rotation = _yawNode.RotationDegrees;
		//rotation.Y = _yaw;
		rotation.Y = Mathf.Lerp(rotation.Y, _yaw, _acceleration * (float)delta);
		_yawNode.RotationDegrees = rotation;
		
		rotation = _pitchNode.RotationDegrees;
		//rotation.X = _pitch;
		rotation.X = Mathf.Lerp(rotation.X, _pitch, _acceleration * (float)delta);
		_pitchNode.RotationDegrees = rotation;


	}

    public override void _Input(InputEvent @event)
    {
		if (@event is InputEventMouseMotion mouseMotion)
		{
			_yaw -= mouseMotion.Relative.X * _sensitivity;
			_pitch -= mouseMotion.Relative.Y * _sensitivity;
		}
    }
}