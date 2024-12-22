using Godot;
using System;

// This script is used to manage the game, such as the player, enemies, and other game objects.
public partial class GameController : Node
{
    /* Componants */
    private Control _inputSettingsMenu;
    
    /* Variables */
    private bool _isPaused = false;

    /* Signals */
    [Signal]
    public delegate void TriggerPauseEventHandler(bool isPaused);



    /* Godot methods */
    public override void _Ready()
    {
        _inputSettingsMenu = GetNode<Control>("/root/GameRoot/GUI/InputSettings");

        // Hide the mouse cursor
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _inputSettingsMenu.Visible = false;
    }

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("pause_menu"))
        {
            TogglePauseMenu();
        }// ...
    }



    /* Toggle menus */
    private void TogglePauseMenu()
    {
        _isPaused = !_isPaused;

        Input.SetMouseMode(_isPaused ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured);
        _inputSettingsMenu.Visible = _isPaused;
        Engine.TimeScale = _isPaused ? 0 : 1;

        EmitSignal(nameof(TriggerPause), _isPaused);
    }
}