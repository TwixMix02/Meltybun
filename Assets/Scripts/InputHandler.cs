using System.Collections.Generic;
using UnityEngine;

public class InputHandler
{
    private Dictionary<string, ICommand> commands;

    
    public InputHandler()
    {
        commands = new Dictionary<string, ICommand>();

        // Initialize keybindings based on what's saved in KeybindingManager
        KeybindingManager.Instance.LoadKeybindings(); // Ensure the keybindings are loaded
    }

    public void BindCommand(string action, ICommand command)
    {
        commands[action] = command;
    }

    public void HandleInput()
{
    if (KeybindingManager.Instance == null)
    {
        Debug.LogError("KeybindingManager.Instance is null.");
        return;
    }

    foreach (var entry in commands)
    {
        KeyCode primaryKey = KeybindingManager.Instance.GetPrimaryKey(entry.Key);
        KeyCode secondaryKey = KeybindingManager.Instance.GetSecondaryKey(entry.Key);

        if (Input.GetKey(primaryKey) || Input.GetKey(secondaryKey))
        {
            entry.Value?.Execute();
        }
    }
}

}
