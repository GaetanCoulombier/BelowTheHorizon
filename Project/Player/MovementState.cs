
using Godot;
using MonoCustomResourceRegistry;

[Tool]
[RegisteredType(nameof(MovementState), "", nameof(Resource))]
public partial class MovementState : Resource
{
    [Export] public int Id { get; set; }
    [Export] public float MovementSpeed { get; set; }
    [Export] public float Acceleration { get; set; } = 6f;
    [Export] public float CameraFov { get; set; } = 75f;
    [Export] public float AnimationSpeed { get; set; } = 1f;
}