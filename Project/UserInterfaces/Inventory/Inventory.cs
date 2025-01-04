using System;
using System.Linq;
using Godot;

public partial class Inventory : Control
{
    [Export] private GridContainer _inventoryGrid;
    private InventorySlot[] _slots;
    private InventorySlot _currentSlot = null;
    private bool _isInventoryOpen = false;

    /* Godot methods */
    public override void _Ready()
    {
        _slots = _inventoryGrid.GetChildren().OfType<InventorySlot>().ToArray();
        
        foreach (var slot in _slots)
        {
            slot.SetSlotID(Array.IndexOf(_slots, slot));
            slot.Connect(nameof(InventorySlot.MouseHover), new Callable(this, nameof(OnSlotHovered)));
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (!GameState.isInventoryOpen)
            return;

        if (_currentSlot == null)
            return;

        // Mouse input
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                GD.Print("Use item");
            }
            
            if (mouseEvent.ButtonIndex == MouseButton.Right)
            {
                GD.Print("Move item");
            }
        }

        // Keyboard input
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (Input.IsActionJustPressed("drop_item"))
            {
                GD.Print("Drop item");
            }
            if (Input.IsActionJustPressed("ui_quick_access_1"))
            {
                GD.Print("Move item");
            }
            if (Input.IsActionJustPressed("ui_quick_access_2"))
            {
                GD.Print("Move item");
            }
            if (Input.IsActionJustPressed("ui_quick_access_3"))
            {
                GD.Print("Move item");
            }
            if (Input.IsActionJustPressed("ui_quick_access_4"))
            {
                GD.Print("Move item");
            }
        }
    }


    


    /* Item management */
    public bool AddItem(BTHItem item)
    {
        var slot = GetFirstEmptyOrSameItemSlot(item);
        if (slot == null)
            return false;

        if (slot.IsEmpty()) {
            slot.SetItem(item);
        } else {
            slot.AddToStack();
        }
        return true;
    }

    private InventorySlot GetFirstEmptyOrSameItemSlot(BTHItem item)
    {
        foreach (var slot in _slots)
        {
            if (slot.IsItem(item) && slot.CanTakeItem(item))
                return slot;

            if (slot.IsEmpty())
                return slot;
        }

        return null;
    }

    private void ReorderSlots()
    {
        // TODO: Implement
    }

    public void Clear()
    {
        foreach (var slot in _slots)
        {
            slot.Clear();
        }
    }



    /* Signals */
    private void OnSlotHovered(int slotID)
    {
        if (slotID < 0 || slotID >= _slots.Length)
            return;

        _currentSlot = _slots[slotID];
    }

    private void OnInventoryChanged(bool isOpen)
    {
        _isInventoryOpen = isOpen;
    }
}