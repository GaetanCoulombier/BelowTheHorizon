using Godot;

public partial class Player : CharacterBody3D
{
    [Export] private Godot.Collections.Dictionary<string, Variant> MovementState { get; set; } = new Godot.Collections.Dictionary<string, Variant>();
    [Export] private Vector3 _movementDirection;

    [Signal] public delegate void SetMovementStateEventHandler(MovementState state);
    [Signal] public delegate void SetMovementDirectionEventHandler(Vector3 movementDirection);


    public override void _Ready()
    {
        MovementState.Add("walk", GD.Load<MovementState>("res://Project/Player/MovementStates/walk.tres"));
        MovementState.Add("run", GD.Load<MovementState>("res://Project/Player/MovementStates/run.tres"));
        MovementState.Add("sprint", GD.Load<MovementState>("res://Project/Player/MovementStates/sprint.tres"));
        MovementState.Add("stand", GD.Load<MovementState>("res://Project/Player/MovementStates/stand.tres"));

        EmitSignal(nameof(SetMovementStateEventHandler), MovementState["stand"]);
    }

    public override void _Input(InputEvent @event)
    {
        // Change the if
        switch (@event.AsText())
        {
            case "move_left":
            case "move_right":
            case "move_forward":
            case "move_backward":
                _movementDirection.X = Input.GetActionStrength("move_left") - Input.GetActionStrength("move_right");
                _movementDirection.Z = Input.GetActionStrength("move_forward") - Input.GetActionStrength("move_backward");

                if (IsMovementOnGoing()){
                    if (Input.IsActionPressed("sprint"))
                    {
                        EmitSignal(nameof(SetMovementStateEventHandler), MovementState["sprint"]);
                    }
                    else {
                        if (Input.IsActionPressed("run"))
                        {
                            EmitSignal(nameof(SetMovementStateEventHandler), MovementState["run"]);
                        }
                        else
                        {
                            EmitSignal(nameof(SetMovementStateEventHandler), MovementState["walk"]);
                        }
                    }
                }
                else
                {
                    EmitSignal(nameof(SetMovementStateEventHandler), MovementState["stand"]);
                }
                break;
            default:
                break;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        EmitSignal(nameof(SetMovementDirectionEventHandler), _movementDirection);
    }

    private bool IsMovementOnGoing()
    {
        return Mathf.Abs(_movementDirection.X) > 0 || Mathf.Abs(_movementDirection.Z) > 0;
    }
}