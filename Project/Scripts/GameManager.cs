using Godot;
using System;

public partial class GameManager : Node
{
    private World _world;
    private CharacterBody3D _player;

    public override void _Ready()
    {
        // Initialisation de la grille
        _world = new World(20, 20, 20);
        AddChild(_world);
    }
}