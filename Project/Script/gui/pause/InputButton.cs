using Godot;

public partial class InputButton : Button
{
    private Label _labelAction;
    private Label _labelInput;

    public InputButton()
    {
        _labelAction = new Label();
        _labelInput = new Label();
    }

    public override void _Ready()
    {
        var previousLabelAction = _labelAction.Text;
        var previousLabelInput = _labelInput.Text;

        _labelAction = GetNode<Label>("MarginContainer/HBoxContainer/LabelAction");
        _labelInput = GetNode<Label>("MarginContainer/HBoxContainer/LabelInput");

        _labelAction.Text = previousLabelAction;
        _labelInput.Text = previousLabelInput;
    }

    public void SetActionData(string actionName, string inputKey)
    {
        _labelAction.Text = actionName;
        _labelInput.Text = inputKey;
    }
}
