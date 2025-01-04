
using Godot;

public partial class UserInterfaceController : Node
{
    [Export] private PlayerController _player;
    [Export] private Control Crosshair;
    [Export] private Control _uiInteractPrompt;
    [Export] private Control _uiInventory;
    [Export] private Control _uiPauseMenu;
    [Export] private Control _uiHUD;
    [Export] private Control _mainMenu; // Not implemented yet

    public override void _Ready()
    {
        // Hide the mouse cursor
        Input.MouseMode = Input.MouseModeEnum.Captured;

        // Hide all UI elements
        _uiInteractPrompt.Visible = false;
        _uiInventory.Visible = false;
        _uiPauseMenu.Visible = false;
        _uiHUD.Visible = true;
    }

    public override void _Process(double delta)
    {
        // TODO : Prevent player from oppening multiple UIs at the same time

        // Check for player input
        if (Input.IsActionJustPressed("open_inventory"))
        {
            ToggleInventory();
        }
        else if (Input.IsActionJustPressed("open_pause_menu"))
        {
            SetGamePauseMenu();
        }
    }

    private void ToggleInventory()
    {
        _uiInventory.Visible = !_uiInventory.Visible;
        GameState.SetMouseMode(_uiInventory.Visible);
        GameState.SetGamePause(false);

        // Manage other UIs
        _uiPauseMenu.Visible = false;
        _uiHUD.Visible = !_uiInventory.Visible;

        GameState.SetInventoryOpen(_uiInventory.Visible);
    }

    private void SetGamePauseMenu()
    {
        _uiPauseMenu.Visible = !_uiPauseMenu.Visible;
        GameState.SetMouseMode(_uiPauseMenu.Visible);
        GameState.SetGamePause(_uiPauseMenu.Visible);

        // Manage other UIs
        _uiInventory.Visible = false;
        _uiHUD.Visible = !_uiPauseMenu.Visible;

        GameState.SetInventoryOpen(_uiInventory.Visible);
    }
}