using Godot;
using System;

public partial class CameraController : Node3D
{
	/* Componants */
    private Vector2 _rotation = Vector2.Zero;
	private Camera3D _camera;
	private Node3D _player;

	/* Variables */
    private float _sensitivity = 0.1f; // TODO : Add this to the settings menu
    private float _maxVerticalAngle = 90; // TODO : Add this to the settings menu

	public override void _Ready()
	{
		_camera = GetNode<Camera3D>("SpringArm3D/Camera3D");
		_player = GetParent<Node3D>();
	}

	public override void _Process(double delta)
	{
		if (_player != null)
        {
            UpdateCameraPosition();
        }

	}

	private void UpdateCameraPosition()
	{
		// Position cible : le joueur
		Vector3 playerPosition = _player.GlobalTransform.Origin;

		// Calcul de la rotation de la caméra
		Basis yawRotation = new Basis(Vector3.Up, Mathf.DegToRad(_rotation.Y)); // Rotation autour de l'axe Y (horizontal)
		Basis pitchRotation = new Basis(Vector3.Right, Mathf.DegToRad(_rotation.X)); // Rotation autour de l'axe X (vertical)
		Basis cameraRotation = yawRotation * pitchRotation; // Combinaison des rotations

		// Calcul du décalage (appliqué au vecteur unité)
		Vector3 cameraOffset = cameraRotation * Vector3.One;

		// Mise à jour de la position globale de la caméra
		GlobalPosition = playerPosition + cameraOffset;

		// Orienter la caméra vers le joueur
		LookAt(playerPosition, Vector3.Up);
	}


     public override void _UnhandledInput(InputEvent @event)
    {
        HandleMouseInput(@event);
    }

    private void HandleMouseInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseEvent)
        {
            // Modify the camera rotation based on the mouse movement
            Vector2 mouseDelta = mouseEvent.Relative;
            _rotation.X -= mouseDelta.Y * _sensitivity;
            _rotation.Y -= mouseDelta.X * _sensitivity;

            // Clamp the vertical rotation angle to prevent the camera from flipping
            _rotation.X = Mathf.Clamp(_rotation.X, -_maxVerticalAngle, _maxVerticalAngle);
        }
    }
}