using Godot;
using System.Linq;

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

    public bool SetSlot(int index, BTHItem item)
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