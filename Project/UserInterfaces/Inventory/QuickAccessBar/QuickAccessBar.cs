using Godot;
using System;
using System.Linq;

public partial class QuickAccessBar : Control
{
	[Export] private GridContainer _actionBarGrid;
	private InventorySlot[] _slots;
	private int _currentSlot = -1;
	private InventorySlot _lastSlot;

	public override void _Ready()
	{
		_slots = _actionBarGrid.GetChildren().OfType<InventorySlot>().ToArray();
        
        foreach (var slot in _slots)
        {
			GD.Print(slot);
            slot.SetSlotID(Array.IndexOf(_slots, slot));
        }
	}

	public override void _Input(InputEvent @event)
	{
		if (GameState.isActionsBlocked) 
			return;

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
					SetCurrentSlot(-1);
					break;
			}
		}

		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			// Check for quick access key presses dynamically
			for (int i = 1; i <= _slots.Length; i++)
			{
				string actionName = $"ui_quick_access_{i}";
				if (Input.IsActionJustPressed(actionName))
				{
					SetCurrentSlot(i - 1);
					break;
				}
			}
		}
	}

	private void ChangeSlot(int direction)
	{
		_currentSlot += direction;
		SetCurrentSlot((_currentSlot + _slots.Length) % _slots.Length); // Loop around the array
	}

	private void SetCurrentSlot(int slot)
	{
		_lastSlot?.RemoveHighlight();

		_currentSlot = slot;

		// Highlight the new slot
		_lastSlot = GetCurrentSlot();
		_lastSlot?.SetHighlight();
	}


	public InventorySlot GetCurrentSlot()
	{
		if (_currentSlot < 0 || _currentSlot >= _slots.Length)
			return null;

		return _slots[_currentSlot];
	}
}
