using Godot;
using System.Linq;

public partial class QuickAccessBar : ASlotContainer
{
    /* Variables */
    private Slot currentSlelectedSlot = null;

    /* Godot methods */
	// Inputs to change the current slot
    public override void _Input(InputEvent @event)
    {
        if (GameState.isActionsBlocked) 
            return;

        // Mouse wheel inputs
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            switch (mouseEvent.ButtonIndex)
            {
                case MouseButton.WheelUp:
                    ChangeSlot(-1);
                    break;

                case MouseButton.WheelDown:
                    ChangeSlot(1);
                    break;

                case MouseButton.Middle:
                    SetCurrentSelectedSlot(null);
                    break;
            }
        }

        // Keyboard 1, 2, 3, 4 inputs
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            for (int i = 1; i <= _slots.Length; i++)
            {
                string actionName = $"ui_quick_access_{i}";
                if (Input.IsActionJustPressed(actionName))
                {
                    SetCurrentSelectedSlot(_slots[i - 1]);
                    break;
                }
            }
        }
    }





    /* Managing the current slots */
    private void ChangeSlot(int direction)
    {
        if (_slots.Length == 0)
            return;

        // Get the index of the current slot
        int currentIndex = currentSlelectedSlot != null ? currentSlelectedSlot.GetSlotID() : -1;

        // Calculate the new slot index
        int newIndex = (currentIndex + direction + _slots.Length) % _slots.Length;

        SetCurrentSelectedSlot(_slots[newIndex]);
    }

    private void SetCurrentSelectedSlot(Slot newSlot)
    {
        // Remove highlight from the current slot
        currentSlelectedSlot?.RemoveHighlight();

        // Update the current slot
        currentSlelectedSlot = newSlot;

        // Highlight the new slot
        currentSlelectedSlot?.SetHighlight();
    }

    // Get the player hand slot for the current selected slot
    public Slot GetCurrentSelectedSlot()
    {
        return currentSlelectedSlot;
    }
}
