using System;
using Godot;

public partial class Player : CharacterBody3D
{
    private PlayerMovementHandler _movementHandler;
    private Camera3D _camera;

    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("/root/GameRoot/Player/Camera");
        
        _movementHandler = new PlayerMovementHandler(this, _camera, 5.0f, -9.81f, 5.0f);
    }

    public override void _PhysicsProcess(double delta)
    {
        _movementHandler.HandleMovement(delta);
    }
}
