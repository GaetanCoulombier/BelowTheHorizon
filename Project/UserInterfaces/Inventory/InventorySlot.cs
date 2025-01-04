using System;
using Godot;

public partial class InventorySlot : Button
{
    /* Signals */
    [Signal] public delegate void MouseHoverEventHandler(int slotID);

    /* Variables */
    [Export] private TextureRect _icon;
    [Export] private Label _stackSizeLabel;
    public int slotID {get; private set;} = -1;

    private BTHItem _item;
    private int _stackSize;



    /* Godot methods */
    public override void _Ready()
    {
        _icon.Texture = null;
        SetStackSize(0);

        Connect("mouse_entered", new Callable(this, nameof(OnMouseEntered)));
        Connect("mouse_exited", new Callable(this, nameof(OnMouseExited)));
    }

    public void SetSlotID(int id)
    {
        slotID = id;
    }



    /* Ckeckers */
    public bool CanTakeItem(BTHItem item)
    {
        return IsEmpty() || IsItem(item) && !IsFull();
    }

    public bool IsItem(BTHItem item)
    {
        return _item == item;
    }

    public bool IsEmpty()
    {
        return _item == null;
    }

    public bool IsFull()
    {
        return _stackSize == _item.GetMaxStackSize();
    }



    /* Item management */
    public void SetItem(BTHItem item)
    {
        if (!IsEmpty())
            return;

        _item = item;
        _icon.Texture = item.GetIcon();
        SetStackSize(1);
    }

    public void Clear()
    {
        if (IsEmpty())
            return;

        _item = null;
        _icon.Texture = null;
        SetStackSize(0);
    }

    public void TransferTo(InventorySlot targetSlot)
    {
        if (IsEmpty())
            return;

        // Cas 1 : Fusionner si même item et slot destination pas plein
        if (targetSlot.IsItem(_item) && !targetSlot.IsFull())
        {
            int transferable = Math.Min(_stackSize, _item.GetMaxStackSize() - targetSlot._stackSize);
            targetSlot.SetStackSize(targetSlot._stackSize + transferable);
            SetStackSize(_stackSize - transferable);

            if (_stackSize == 0)
                Clear();

            return;
        }

        // Cas 2 : Échanger les contenus
        var tempItem = targetSlot._item;
        var tempStackSize = targetSlot._stackSize;

        targetSlot.SetItem(_item);
        targetSlot.SetStackSize(_stackSize);

        SetItem(tempItem);
        SetStackSize(tempStackSize);
    }




    /* Stack management */
    public void AddToStack()
    {
        if (IsFull())
            return;

        SetStackSize(_stackSize + 1);
    }

    private void SetStackSize(int value)
    {
        _stackSize = value;

        if (value == 1 || value == 0)
            _stackSizeLabel.Text = "";
        else
            _stackSizeLabel.Text = value.ToString();
    }


    /* Highlight */
    public void SetHighlight()
    {
        Modulate = new Color(0.8f, 1f, 0.5f); // Couleur mise en surbrillance
    }

    public void RemoveHighlight()
    {
        Modulate = new Color(1, 1, 1); // Couleur par défaut
    }


    // Combiner les inputs 1, 2, 3, 4 avec le hover
    // Click gauche : utiliser item
    // Click droit : déplacer item

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