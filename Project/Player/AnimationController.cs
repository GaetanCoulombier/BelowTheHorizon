using Godot;

public partial class AnimationController : Node
{
    /* Componants */
    private PlayerController _playerController;
    private AnimationTree _animationTree;
    private AnimationNodeStateMachinePlayback _animationPlayback;

    /* Variable */
    private bool _isFalling = false;

    /* Godot methods */
    public override void _Ready()
    {
        _playerController = GetParent<PlayerController>();
        _animationTree = _playerController.GetNode<AnimationTree>("AnimationTree");
        _animationTree.Set("parameters/grounded", true);
        _animationPlayback = (AnimationNodeStateMachinePlayback) _animationTree.Get("parameters/playback");
        
        // Connect the signals
        _playerController.Connect(nameof(PlayerController.ChangeMovementState), new Callable(this, nameof(OnChangeMovementState)));
    }

    public override void _PhysicsProcess(double delta)
    {
        // Do something
        if (_playerController.IsOnFloor() && _isFalling)
        {
            _isFalling = false;
            _animationPlayback.Travel("landing");
        }
    }

    private void OnChangeMovementState(MovementState movementState)
    {
        GD.Print("Change movement state to " + movementState.ToString());

        if (movementState == MovementState.WALK)
        {
            _animationPlayback.Travel("walking");
        }
        else if (movementState == MovementState.RUN)
        {
            _animationPlayback.Travel("running");
        }
        else if (movementState == MovementState.CROUCH)
        {
            _animationPlayback.Travel("crouching");
        }
        else if (movementState == MovementState.IDLE)
        {
            _animationPlayback.Travel("idle");
        }
        else if (movementState == MovementState.FALL && !_isFalling)
        {
            _animationPlayback.Travel("falling_idle");
            _isFalling = true;
        }
    }
}