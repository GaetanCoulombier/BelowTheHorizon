using Godot;
using System;

// This script is used to manage the game, such as the player, enemies, and other game objects.
public partial class GameManager : Node
{
    private CharacterBody3D _player;

    public override void _Ready()
    {
        _player = GetNode<CharacterBody3D>("/root/GameRoot/Player");
        GD.Print(_player.GetChildren());
    }
}