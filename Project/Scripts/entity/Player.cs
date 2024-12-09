using Godot;

public partial class Player : CharacterBody3D
{
    private float _speed = 5.0f; // Vitesse du joueur
    private Vector3 _velocity = Vector3.Zero; // Stocke la vélocité réelle
    private float _gravity = -9.81f; // Gravité
    private float _jumpStrength = 5.0f; // Force de saut

    private Camera3D _camera; // Référence à la caméra

    public override void _Ready()
    {
        GlobalPosition = new Vector3(1.5f, 5f, 1.5f); // Position initiale
        SetPhysicsProcess(true); // Active la physique

        // Trouver la caméra dans la hiérarchie de la scène
        _camera = GetNode<Camera3D>("/root/GameRoot/Camera");
    }

    public override void _PhysicsProcess(double delta)
    {
        // Appliquer la gravité si nécessaire
        ApplyGravity(delta);

        // Calculer le mouvement basé sur l'entrée utilisateur
        Vector3 inputMovement = GetInputMovement();

        // Appliquer le mouvement horizontal (X et Z uniquement)
        Vector3 horizontalMovement = new Vector3(inputMovement.X, 0, inputMovement.Z) * _speed;

        // Transformer le mouvement pour qu'il corresponde à l'orientation de la caméra
        Vector3 globalMovement = TransformDirection(horizontalMovement);

        _velocity.X = globalMovement.X;
        _velocity.Z = globalMovement.Z;

        // Appliquer la vélocité verticale
        Velocity = _velocity;

        // Déplacer le joueur
        MoveAndSlide();
    }

    private Vector3 GetInputMovement()
    {
        Vector3 inputVector = Vector3.Zero;

        if (Input.IsActionPressed("Move_Forward")) inputVector.Z -= 1;
        if (Input.IsActionPressed("Move_Backward")) inputVector.Z += 1;
        if (Input.IsActionPressed("Move_Left")) inputVector.X -= 1;
        if (Input.IsActionPressed("Move_Right")) inputVector.X += 1;

        if (Input.IsActionJustPressed("Move_Jump") && IsOnFloor())
        {
            _velocity.Y = _jumpStrength; // Saut
        }

        return inputVector.Normalized(); // Normaliser pour éviter une vitesse excessive
    }

    private Vector3 TransformDirection(Vector3 localDirection)
    {
        // Obtient la base de rotation de la caméra pour calculer les directions
        Basis cameraBasis = _camera.GlobalTransform.Basis;

        // Ignore la composante verticale (caméra qui regarde en haut ou en bas)
        cameraBasis = new Basis(cameraBasis.X, Vector3.Up, cameraBasis.Z);

        // Transforme la direction locale en une direction globale
        return cameraBasis * localDirection;
    }

    private void ApplyGravity(double delta)
    {
        if (!IsOnFloor())
        {
            _velocity.Y += _gravity * (float)delta; // Appliquer la gravité si en l'air
        }
        else if (_velocity.Y < 0)
        {
            _velocity.Y = 0; // Réinitialiser la vélocité verticale au sol
        }
    }
}
