using Godot;
using System;

public partial class Camera : Camera3D
{
    private Node3D _player;
    private Vector3 _offset = new Vector3(0, 3, 6); // Position relative de la caméra par rapport au joueur
    private Vector2 _rotation = Vector2.Zero; // Stocke la rotation de la caméra
    private float _sensitivity = 0.1f; // Sensibilité de la souris
    private float _maxVerticalAngle = 45; // Angle vertical maximal

    public override void _Ready()
    {
        // Trouver le joueur dans la scène (assurez-vous que le joueur s'appelle "Player")
        _player = GetNode<Node3D>("/root/GameRoot/Player");

        // Cacher le curseur pour une meilleure immersion
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_player != null)
        {
            UpdateCameraPosition(); // Met à jour la position de la caméra
            RotatePlayerToFaceCamera(); // Aligne le joueur avec la caméra horizontalement
        }
    }

    private void UpdateCameraPosition()
    {
        // Position cible de la caméra (position du joueur)
        Vector3 targetPosition = _player.GlobalTransform.Origin;

        // Crée une base de rotation à partir des angles
        Basis rotationBasis = new Basis(new Vector3(0, 1, 0), Mathf.DegToRad(_rotation.Y)) // Rotation horizontale
                            * new Basis(new Vector3(1, 0, 0), Mathf.DegToRad(_rotation.X)); // Rotation verticale

        // Applique la rotation à l'offset pour calculer la nouvelle position de la caméra
        Vector3 rotatedOffset = rotationBasis * _offset;

        // Met à jour la position de la caméra
        GlobalPosition = targetPosition + rotatedOffset;

        // Oriente la caméra vers le joueur
        LookAt(targetPosition, Vector3.Up);
    }

    private void RotatePlayerToFaceCamera()
    {
        // Aligne le joueur avec la direction horizontale de la caméra
        Vector3 directionToCamera = GlobalPosition - _player.GlobalTransform.Origin;
        directionToCamera.Y = 0; // Ignore la composante verticale
        directionToCamera = directionToCamera.Normalized(); // Normalise le vecteur

        // Oriente le joueur pour qu'il soit dos à la caméra
        _player.LookAt(_player.GlobalTransform.Origin - directionToCamera, Vector3.Up);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseEvent)
        {
            // Obtenez les mouvements de la souris
            Vector2 mouseDelta = mouseEvent.Relative;

            // Ajustez les angles de rotation en fonction de la sensibilité
            _rotation.X -= mouseDelta.Y * _sensitivity;
            _rotation.Y -= mouseDelta.X * _sensitivity;

            // Limitez l'angle vertical pour éviter que la caméra ne fasse un tour complet
            _rotation.X = Mathf.Clamp(_rotation.X, -_maxVerticalAngle, _maxVerticalAngle);
        }

        // Permet de libérer le curseur en appuyant sur Échap
        if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.Escape)
        {
            Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured
                ? Input.MouseModeEnum.Visible
                : Input.MouseModeEnum.Captured;
        }
    }
}
