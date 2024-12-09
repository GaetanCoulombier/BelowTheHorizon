using Godot;
using System;

public partial class CameraFollow : Camera3D
{
    private Node3D _player;

    public override void _Ready()
    {
        // Trouver le joueur dans la scène (assurez-vous que le joueur s'appelle "Player")
        _player = GetNode<Node3D>("/root/GameRoot/Player");
    }

    public override void _PhysicsProcess(double delta)
    {
        // Assurez-vous que la caméra suit le joueur à une position donnée
        if (_player != null)
        {
            // Suivre le joueur tout en maintenant la position de la caméra à une certaine hauteur et distance
            GlobalPosition = new Vector3(_player.GlobalPosition.X + 5, _player.GlobalPosition.Y + 5, _player.GlobalPosition.Z + 10);
            LookAt(_player.GlobalPosition, Vector3.Up); // Regarder vers le joueur
        }
    }
}
