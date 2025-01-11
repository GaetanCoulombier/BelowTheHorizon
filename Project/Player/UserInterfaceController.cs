
using System;
using Godot;

public partial class UserInterfaceController : Node
{
    /* Nodes */
    [Export] private PlayerController _player;
    [Export] private Control _uiHUD;
    [Export] private Control _crosshair;
    [Export] private Control _uiInteractPrompt;
    [Export] private QuickAccessBar _uiQuickAccessBar;
    [Export] private Inventory _uiInventory;
    [Export] private Control _uiPauseMenu;
    [Export] private Control _mainMenu; // Not implemented yet

    /* Variables */
    private ASlotContainer _currentInteractInventory = null;
    private ASlotContainer _currentOpennedInventory = null;


    /* Godot methods */
    public override void _Ready()
    {
        // Hide the mouse cursor
        Input.MouseMode = Input.MouseModeEnum.Captured;

        // Hide all UI elements
        _uiInteractPrompt.Visible = false;
        _uiInventory.Visible = false;
        _uiPauseMenu.Visible = false;
        _uiHUD.Visible = true;
        _crosshair.Visible = true;
        _uiQuickAccessBar.Visible = true;

        // Connect signals
        _uiInventory.Connect(nameof(ASlotContainer.SetCurrentInventory), new Callable(this, nameof(OnSetCurrentInteractInventory)));
        _uiQuickAccessBar.Connect(nameof(ASlotContainer.SetCurrentInventory), new Callable(this, nameof(OnSetCurrentInteractInventory)));
    }

    public override void _Process(double delta)
    {
        // TODO : Prevent player from oppening multiple UIs at the same time

        // Check for player input
        if (Input.IsActionJustPressed("open_inventory"))
        {
            ToggleInventory(_uiInventory);
        }
        else if (Input.IsActionJustPressed("open_pause_menu"))
        {
            SetGamePauseMenu();
        }
    }





    /* Open menus methods */
    private void ToggleInventory(ASlotContainer inventory)
    {
        _uiInventory.Visible = !_uiInventory.Visible;
        GameState.SetMouseMode(_uiInventory.Visible);
        GameState.SetGamePause(false);

        // Manage other UIs
        _uiPauseMenu.Visible = false;
        _crosshair.Visible = !_uiInventory.Visible;
        _uiInteractPrompt.Visible = !_uiInventory.Visible;

        // Visible inventory
        _currentOpennedInventory = inventory;

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
        _crosshair.Visible = !_uiPauseMenu.Visible;
        _uiInteractPrompt.Visible = !_uiPauseMenu.Visible;

        GameState.SetInventoryOpen(_uiInventory.Visible);
    }





    /* Inputs for inside of the inventory */
    public override void _Input(InputEvent @event)
    {
        if (!GameState.isInventoryOpen)
            return;

        var currentSlot = _currentInteractInventory?.GetCurrentSlot();

        if (currentSlot == null)
            return;

        // Mouse input
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            
            // Move item to the opposite inventory
            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                if (_currentInteractInventory is QuickAccessBar) {
                    _currentOpennedInventory.SwitchFirst(currentSlot);
                    _currentOpennedInventory.ReorderSlots();
                } else {
                    _uiQuickAccessBar.SwitchFirst(currentSlot);
                }
            }

            // Use the item
            if (mouseEvent.ButtonIndex == MouseButton.Right)
            {
                currentSlot.GetItem().UseSecondary(_player);
            }
        }

        // Keyboard input
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (Input.IsActionJustPressed("drop_item"))
            {
                currentSlot.Drop();
            }

            // Check for quick access key presses dynamically
            for (int i = 1; i <= _uiQuickAccessBar.GetSlots().Length; i++)
            {
                string actionName = $"ui_quick_access_{i}";
                if (Input.IsActionJustPressed(actionName))
                {
                    currentSlot.SwapWith(_uiQuickAccessBar.GetSlot(i - 1));
                    break;
                }
            }
        }
    }





    /* Signals */
    private void OnSetCurrentInteractInventory()
    {
        // Update the current slot container
        _currentInteractInventory = ASlotContainer.currentInventory;
    }
}