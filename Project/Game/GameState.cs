using System;
using Godot;

// This script is used to manage the game, such as the player, enemies, and other game objects.
public partial class GameState : Node
{
    /* Singleton */
    private static GameState _instance;

    /* Variables */
    public static bool isPaused {get; private set;}
    public static bool isActionsBlocked {get; private set;}
    public static bool isInventoryOpen {get; private set;}

    public override void _Ready()
    {
        _instance = this;

        SetGamePause(false);
        SetMouseMode(false);
    }


    public static GameState GetInstance()
    {
        return _instance;
    }

    public static void SetGamePause(bool value)
    {
        // Set the game to pause or unpause
        isPaused = value;
        Engine.TimeScale = value ? 0 : 1;
    }

    public static void SetMouseMode(bool value)
    {
        Input.SetMouseMode(value ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured);
        isActionsBlocked = value;
    }

    public static void SetInventoryOpen(bool value)
    {
        isInventoryOpen = value;
    }
}