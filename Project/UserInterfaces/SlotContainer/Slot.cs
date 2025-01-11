using Godot;
using BelowTheHorizon;

public partial class Slot : Godot.Button
{
    /* Signals */
    [Signal] public delegate void MouseHoverEventHandler(int slotID);

    /* Variables */
    [Export] private TextureRect _icon;
    public int slotID {get; private set;} = -1;

    // Set when the slot is added to a container (chest, inventory, quick access bar)
    private Item _item = null;
    private ASlotContainer slotContainer = null;



    /* Godot methods */
    public override void _Ready()
    {
        _icon.Texture = null;

        Connect("mouse_entered", new Callable(this, nameof(OnMouseEntered)));
        Connect("mouse_exited", new Callable(this, nameof(OnMouseExited)));
    }



    /* Getters & Setters */
    public void SetSlotID(int id)
    {
        slotID = id;
    }
    public int GetSlotID()
    {
        return slotID;
    }
    public Item GetItem()
    {
        return _item;
    }
    public void SetSlotContainer(ASlotContainer container)
    {
        slotContainer = container;
    }





    /* Ckeckers */
    public bool CanTakeItem(Item item)
    {
        return IsEmpty() || IsItem(item);
    }

    public bool IsItem(Item item)
    {
        return _item == item;
    }

    public bool IsEmpty()
    {
        return _item == null;
    }





    /* Item management */
    public void SwapWith(Slot swappedSlot)
    {
        Item tempItem = swappedSlot.GetItem();

        swappedSlot.SetItem(_item);
        SetItem(tempItem);
    }

    public void SetItem(Item item)
    {
        _item = item;
        _icon.Texture = item?.GetIcon();
        if (item == null) Clear();
    }

    public void Drop()
    {
        if (IsEmpty())
            return; // Nothing to drop

        // Check if the slot is in a container that has a player (Inventory, QuickAccessBar)
        var _player = slotContainer.GetPlayer();
        if (_player == null)
            return;

        _item.Drop(_player);
        Clear();
    }

    public void Clear()
    {
        if (IsEmpty())
            return;

        _item = null;
        _icon.Texture = null;
    }



    /* Highlighting */
    public void SetHighlight()
    {
        Modulate = new Color(0.8f, 1f, 0.5f); // Couleur mise en surbrillance
    }

    public void RemoveHighlight()
    {
        Modulate = new Color(1, 1, 1); // Couleur par défaut
    }





    /* Signals */
    private void OnMouseEntered()
    {
        EmitSignal(nameof(MouseHover), slotID);
    }

    private void OnMouseExited()
    {
        EmitSignal(nameof(MouseHover), -1);
    }
}