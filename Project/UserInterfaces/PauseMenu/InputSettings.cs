using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class InputSettings : Control
{
    [Export] private PackedScene _inputButtonScene;
    [Export] private Control _actionList;

    private bool _isRemapping = false;
    private InputButton _remappingButton = null;
    private string _remappingAction = null;

    /* Remappable actions */
    private Dictionary<string, string> _allowRemapActions = new Dictionary<string, string>
    {
        { "move_forward", "Move forward" },
        { "move_backward", "Move backward" },
        { "move_left", "Move left" },
        { "move_right", "Move right" },
        { "move_up", "Move up" },
        { "move_down", "Move down" },
        { "jump", "Jump" },
        { "crouch", "Crouch" },
        { "sprint", "Sprint" }
    };

    string GetKeyByValue(Dictionary<string, string> dictionary, string value)
    {
        return dictionary.FirstOrDefault(pair => pair.Value == value).Key;
    }

    /* Remapping */
    public override void _Ready()
    {
        _actionList = GetNode<Control>("/root/GameRoot/GUI/InputSettings/PanelContainer/MarginContainer/VBoxContainer/ScrollContainer/ActionList");
        _inputButtonScene = ResourceLoader.Load<PackedScene>("res://Project/UserInterfaces/PauseMenu/InputButton.tscn");
        var _restButton = GetNode<Button>("/root/GameRoot/GUI/InputSettings/PanelContainer/MarginContainer/VBoxContainer/ResetButton");

        _restButton.Connect("pressed", new Callable(this, nameof(OnResetButtonPressed)));

        CreateActionList();
    }

    private void CreateActionList()
    {
        // Charger la configuration d'InputMap
        InputMap.LoadFromProjectSettings();

        // Supprimer les anciens boutons
        foreach (Node item in _actionList.GetChildren())
            item.QueueFree();

        // Ajouter les actions remappables
        foreach (var action in _allowRemapActions.Keys)
        {
            string actionLabel = _allowRemapActions[action];
            string inputLabel = "";

            var events = InputMap.ActionGetEvents(action);
            if (events.Count > 0)
                inputLabel = events[0].AsText();

            // Instancier et configurer le bouton
            var button = _inputButtonScene.Instantiate<InputButton>();

            // Utiliser CallDeferred pour définir les données après l'initialisation
            button.CallDeferred(nameof(InputButton.SetActionData), actionLabel, inputLabel);

            _actionList.AddChild(button);
            
            button.Connect("RemapRequested", new Callable(this, nameof(OnRemapRequested)));
        }
    }

    private void OnRemapRequested(InputButton button)
    {
        if (_isRemapping)
            return;

        // Début du remapping
        _isRemapping = true;
        _remappingButton = button;
        _remappingAction = GetKeyByValue(_allowRemapActions, button.GetActionLabel()); // We need to get the same name than the godot InputMap action

        // Indiquer visuellement que l'utilisateur doit appuyer sur une touche
        _remappingButton.UpdateInputLabel("Press key to bind");
    }

    public override void _Input(InputEvent @event)
    {
        // Vérify if we are remapping and if the event is a key or mouse button
        if (!_isRemapping || @event is not InputEventKey && @event is not InputEventMouseButton)
            return;


        string inputName = "";

        // KEYBOARD
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            inputName = keyEvent.AsText();
        }

        // MOUSE
        else if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            inputName = "Mouse " + mouseEvent.ButtonIndex.ToString();
        }

        // Remap the action
        InputMap.ActionEraseEvents(_remappingAction);
        InputMap.ActionAddEvent(_remappingAction, @event);
        _remappingButton.UpdateInputLabel(inputName);

        // End of remapping
        _isRemapping = false;
        _remappingButton = null;
        _remappingAction = null;
        
        // Stop the event propagation
        AcceptEvent();

        // Save the new configuration
        SaveProjectSettings();
    }

    private void OnResetButtonPressed()
    {
        CreateActionList();
    }


    private void SaveProjectSettings()
    {
        // TODO: Save the new configuration into a file and load it when the game starts
    }
}
