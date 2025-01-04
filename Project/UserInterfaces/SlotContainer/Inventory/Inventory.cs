using Godot;

public partial class Inventory : ASlotContainer
{

    /* Variables for inventory slots */
    private Slot[] currentSlots;
    private bool _isInventoryOpen = false;



    /* Godot methods */
    // Inputs in the inventory


    
    

    /* Item management in inventory slots */
    public bool AddItem(BTHItem item)
    {
        var slot = GetFirstEmptySlot();

        if (slot == null)
            return false;

        if (!slot.IsEmpty())
            return false;
        
        slot.SetItem(item);
        return true;
    }

    private Slot GetFirstEmptySlot()
    {
        foreach (var slot in _slots)
        {
            if (slot.IsEmpty())
                return slot;
        }

        return null;
    }

    // To avoid empty slots in the middle of the inventory
    private void ReorderSlots()
    {
        // TODO: Implement
    }
}