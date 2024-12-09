using Godot;
using System;

public partial class GameManager : Node
{
    private World _world;
    private CharacterBody3D _player;

    public override void _Ready()
    {
        // Initialisation de la grille
        _world = new World(10, 3, 10);

        // Ajouter des murs
        for (int x = 0; x < _world.Width; x++)
        {
            for (int z = 0; z < _world.Depth; z++)
            {
                SetBlock(new Wall(new Vector3I(x, 0, z)));
            }
        }

        // Ajouter un mur
        SetBlock(new Wall(new Vector3I(2, 1, 2)));

        // Initialiser le joueur
        _player = GetNode<Player>("Player");
    }

    public void SetBlock(Block block)
    {
        if (_world.SetBlock(block))
        {
            AddChild(block); // Add block to the scene
        }
    }
}