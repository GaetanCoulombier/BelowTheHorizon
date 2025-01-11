using Godot;
using System.Linq;
using BelowTheHorizon;

public abstract partial class ASlotContainer : Control
{
    /* Inventory focus */
    private int inventoryID = -1;
    private static int lastInventoryID = -1;
    public static ASlotContainer currentInventory { get; protected set; }
    [Signal] public delegate void SetCurrentInventoryEventHandler();

    /* Nodes */
    [Export] protected GridContainer _gridContainer;
    [Export] protected PlayerController _player;

    /* Variables */
    protected Slot[] _slots;
    public Slot currentSlot { get; protected set; }

    /* Godot Methods */
    public override void _Ready()
    {
        inventoryID = lastInventoryID++;

        _slots = _gridContainer.GetChildren().OfType<Slot>().ToArray();

        InitializeSlots();
    }

    protected void InitializeSlots()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            Slot slot = _slots[i];

            slot.SetSlotID(i);
            slot.SetSlotContainer(this);
            slot.Connect(nameof(Slot.MouseHover), new Callable(this, nameof(OnSlotHovered)));
        }
    }





    /* Getters & Setters */
    public Slot GetCurrentSlot()
    {
        return currentSlot;
    }

    public Slot[] GetSlots()
    {
        return _slots;
    }

    public bool SetSlot(int index, Item item)
    {
        if (index < 0 || index >= _slots.Length)
            return false;

        _slots[index].SetItem(item);
        return true;
    }

	public Slot GetSlot(int index)
	{
		return _slots[index];
	}

    public PlayerController GetPlayer()
    {
        return _player;
    }





    /* Item management in inventory slots */
    public void AddItem(Item item)
    {
        if (!CanAddItem(item))
            return;

        var slot = GetFirstEmptySlot();
        slot.SetItem(item);
    }

    public bool CanAddItem(Item item)
    {
        var slot = GetFirstEmptySlot();

        if (slot == null)
            return false;

        return slot.IsEmpty();
    }

    public void SwitchFirst(Slot swappedSlot)
    {
        var firstSlot = GetFirstEmptySlot();
        firstSlot.SwapWith(swappedSlot);
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

    // Trim the empty slots to the end of the array
    public void ReorderSlots()
    {
        int emptyIndex = 0;

        for (int i = 0; i < _slots.Length; i++)
        {
            if (!_slots[i].IsEmpty())
            {
                if (i != emptyIndex)
                {
                    _slots[emptyIndex].SwapWith(_slots[i]);
                }
                emptyIndex++;
            }
        }
    }






    /* Signals */
    private void OnSlotHovered(int slotID)
    {
        if (slotID < 0 || slotID >= _slots.Length)
            return;

        currentSlot = _slots[slotID];

        // Change the current inventory
        currentInventory = this;
        EmitSignal(nameof(SetCurrentInventory));
    }
}