using Godot;
using System;

// This script is used to manage the game, such as the player, enemies, and other game objects.
public partial class GameManager : Node
{
    private Control _inputSettingsMenu;

    public override void _Ready()
    {
        _inputSettingsMenu = GetNode<Control>("/root/GameRoot/GUI/InputSettings");

        // Hide the mouse cursor
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _inputSettingsMenu.Visible = false;
    }

    // GUI input handling
    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("pause_menu"))
        {
            TogglePauseMenu();
        }// ...
    }

    private void TogglePauseMenu()
    {
        if (GlobalState.IsPaused)
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
            _inputSettingsMenu.Visible = false;
            Engine.TimeScale = 1;
        }
        else
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            _inputSettingsMenu.Visible = true;
            Engine.TimeScale = 0;
        }

        GlobalState.IsPaused = !GlobalState.IsPaused;
    }
}