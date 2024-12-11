using Godot;

public partial class InputSettings : Control
{
    private PackedScene _inputButtonScene = null;
    private Control _actionList = null;

    private bool _isRemapping = false;
    private Button _actionToRemap = null;
    private Button _remappingButton = null;

    public override void _Ready()
    {
        _actionList = GetNode<Control>("/root/GameRoot/GUI/InputSettings/PanelContainer/MarginContainer/VBoxContainer/ScrollContainer/ActionList");
        _inputButtonScene = ResourceLoader.Load<PackedScene>("res://Project/Scenes/UserInterface/InputButton.tscn");
        CreateActionList();
    }

    private void CreateActionList()
    {
        InputMap.LoadFromProjectSettings();
        
        // Clear the action list
        foreach(Node item in _actionList.GetChildren()) { item.QueueFree(); }

        // Add all actions to the list
        foreach(string action in InputMap.GetActions())
        {
            var events = InputMap.ActionGetEvents(action);
            var inputLabel = (events.Count > 0) ? events[0].AsText() : "";

            InputButton button = _inputButtonScene.Instantiate() as InputButton;
            button.SetActionData(action, inputLabel);

            _actionList.AddChild(button);
        }
    }
}
