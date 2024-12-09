using Godot;

public partial class Player : CharacterBody3D
{
    private float _speed = 5.0f;

    public override void _Ready()
    {
        GlobalPosition = new Vector3(1.5f, 5f, 1.5f); // Position flottante
        SetPhysicsProcess(true);
    }

    public override void _PhysicsProcess(double delta)
    {
        ApplyGravity();

        Vector3 movement = GetInputMovement() * (float)delta * _speed;
        Velocity += movement; // Modifie la vélocité pour un mouvement continu

        MoveAndSlide(); // Utilise les collisions natives de Godot
        //GD.Print("Player position: " + GlobalPosition);
    }

    private Vector3 GetInputMovement()
    {
        Vector3 inputVector = Vector3.Zero;

        if (Input.IsActionPressed("move_forward")) inputVector.Z -= 1;
        if (Input.IsActionPressed("move_back")) inputVector.Z += 1;
        if (Input.IsActionPressed("move_left")) inputVector.X -= 1;
        if (Input.IsActionPressed("move_right")) inputVector.X += 1;
        if (Input.IsActionPressed("jump") && IsOnFloor()) inputVector.Y += 1;

        return inputVector.Normalized();
    }

    private void ApplyGravity()
    {
        if (!IsOnFloor())
            Velocity += Vector3.Down * 2.0f;
    }
}

