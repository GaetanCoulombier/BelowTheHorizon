using Godot;

public partial class InputButton : Button
{
    private Label _labelAction;
    private Label _labelInput;

    [Signal]
    public delegate void RemapRequestedEventHandler(InputButton button);
    
    public override void _Ready()
    {
        // Liaison des sous-nœuds
        _labelAction = GetNode<Label>("MarginContainer/HBoxContainer/LabelAction");
        _labelInput = GetNode<Label>("MarginContainer/HBoxContainer/LabelInput");
        
        // Ajout d'un signal pour détecter le clic
        this.Pressed += OnButtonPressed;
    }

    public void SetActionData(string actionName, string inputKey)
    {
        _labelAction.Text = actionName;
        _labelInput.Text = inputKey;
    }

    public void UpdateInputLabel(string newKey)
    {
        _labelInput.Text = newKey;
    }

    public string GetActionLabel()
    {
        return _labelAction.Text;
    }

    private void OnButtonPressed()
    {
        EmitSignal(nameof(RemapRequested), this);
    }
}
