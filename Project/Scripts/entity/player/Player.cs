using System;
using Godot;

public partial class Player : CharacterBody3D
{
    private PlayerMovementHandler _movementHandler; // Gestionnaire de mouvements
    private Camera3D _playerCamera; // Caméra du joueur

    public override void _Ready()
    {
        _playerCamera = GetNode<Camera3D>("/root/GameRoot/Player/Camera");

        // Initialiser le gestionnaire de mouvements avec les paramètres souhaités
        _movementHandler = new PlayerMovementHandler(this, _playerCamera, 5.0f, -9.81f, 5.0f);
    }

    public override void _PhysicsProcess(double delta)
    {
        // Déléguer le contrôle des mouvements au gestionnaire
        _movementHandler.HandleMovement(delta);
    }
}
