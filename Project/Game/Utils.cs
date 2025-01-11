using Godot;
using Godot.Collections;
using System.Linq;

public class Utils
{
    /* Actions possibles in the game */
    public static Dictionary<string, string> _actions {get; private set;} = new Dictionary<string, string>
    {
        { "move_forward", "Move forward" },
        { "move_backward", "Move backward" },
        { "move_left", "Move left" },
        { "move_right", "Move right" },
        { "jump", "Jump" },
        { "sprint", "Sprint" },
        { "crouch", "Crouch" },
        { "crawl", "Crawl" },
        { "interact", "Interact" },
        { "move_up", "Move up" },
        { "move_down", "Move down" },
        { "open_inventory" , "Open the inventory" },
        { "drop_item", "Drop item" },
        { "ui_quick_access_1", "Action bar 1" },
        { "ui_quick_access_2", "Action bar 2" },
        { "ui_quick_access_3", "Action bar 3" },
        { "ui_quick_access_4", "Action bar 4" },
    };
    public static string GetActionKeyByValue(string value)
    {
        return _actions.FirstOrDefault(pair => pair.Value == value).Key;
    }


    /* Get the key associated with an action */
    public static string GetKeyByAction(string action)
    {
        return InputMap.ActionGetEvents(action).Count > 0 ? InputMap.ActionGetEvents(action)[0].AsText() : "";
    }
}