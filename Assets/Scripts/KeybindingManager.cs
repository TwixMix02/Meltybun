using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeybindingManager : MonoBehaviour
{
    public static KeybindingManager Instance;
    public static OptionsMenu menu;

    private Dictionary<string, KeyCode> primaryKeyBindings;
    private Dictionary<string, KeyCode> secondaryKeyBindings;

    private string currentAction = "";

    void Start(){
        primaryKeyBindings.Clear();
        secondaryKeyBindings.Clear();
        menu = GetComponent<OptionsMenu>();
    }

    public void SetPrimaryKey(string action, KeyCode key, OptionsMenu optionsMenu = null)
    {
        if (primaryKeyBindings.ContainsKey(action))
        {
            primaryKeyBindings[action] = key;
        }
        else
        {
            primaryKeyBindings.Add(action, key);
        }
        optionsMenu?.UpdateKeyBindingTexts();
    }

    public void SetSecondaryKey(string action, KeyCode key, OptionsMenu optionsMenu = null)
    {
        if (secondaryKeyBindings.ContainsKey(action))
        {
            secondaryKeyBindings[action] = key;
        }
        else
        {
            secondaryKeyBindings.Add(action, key);
        }
        optionsMenu?.UpdateKeyBindingTexts();
    }



    // Ensure only one instance of KeybindingManager exists
    void Awake()
{
    if (Instance == null)
    {
        Instance = this;
    }
    else
    {
        Destroy(gameObject);
    }

    primaryKeyBindings = new Dictionary<string, KeyCode>();
    secondaryKeyBindings = new Dictionary<string, KeyCode>();
    LoadKeybindings();
}


    // Call to set keybinding for a specific action
    public void SetBinding(string action)
    {
        Debug.Log("Input: " + action);
        currentAction = action;
        StartCoroutine(KeyListeningCoroutine(menu));
    }

    public void updateButton(OptionsMenu optionsMenu = null){
        optionsMenu?.UpdateKeyBindingTexts();
    }

private IEnumerator KeyListeningCoroutine(OptionsMenu optionsMenu = null)
{

    // Wait for primary key input
    yield return StartCoroutine(ListenForKey("primary"));

    // Ensure primary and secondary keys are different
    KeyCode currentPrimary = GetPrimaryKey(currentAction);
    while (true)
    {
        yield return StartCoroutine(ListenForKey("secondary"));

        KeyCode currentSecondary = GetSecondaryKey(currentAction);
        if (currentSecondary != currentPrimary)
        {
            break; // Exit if keys are different
        }

        Debug.LogWarning("Secondary Key cannot be the same as Primary Key. Please select a different key.");
    }

    Debug.Log("Key Listening Complete for Action: " + currentAction);
    optionsMenu?.UpdateKeyBindingTexts();

}


    private IEnumerator ListenForKey(string keyType)
    {
        KeyCode key = KeyCode.None;

        // Wait for a key to be pressed
        while (!Input.anyKeyDown)
        {
            yield return null; // Wait one frame
        }

        foreach (KeyCode k in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(k))
            {
                key = k;
                break;
            }
        }

        if (key != KeyCode.None)
        {
            if (keyType == "primary")
            {
                SetPrimaryKey(currentAction, key);
                Debug.Log("Primary Key Set for " + currentAction + ": " + key);
            }
            else if (keyType == "secondary")
            {
                SetSecondaryKey(currentAction, key);
                Debug.Log("Secondary Key Set for " + currentAction + ": " + key);
            }
        }

        SaveKeybindings(menu); // Save after both primary and secondary keys are set
    }


    // Get the primary key for an action
    public KeyCode GetPrimaryKey(string action)
    {
        if (primaryKeyBindings.ContainsKey(action))
        {
            return primaryKeyBindings[action];
        }
        Debug.Log("Primary Key Failed: " + primaryKeyBindings[action].ToString());
        return KeyCode.None; // Return None if not found
    }

    // Get the secondary key for an action
    public KeyCode GetSecondaryKey(string action)
    {
        if (secondaryKeyBindings.ContainsKey(action))
        {
            return secondaryKeyBindings[action];
        }
        Debug.Log("Secondary Key Failed: " + secondaryKeyBindings[action].ToString());
        return KeyCode.None; // Return None if not found
    }

    // Save keybindings to PlayerPrefs
    public void SaveKeybindings(OptionsMenu optionsMenu = null)
    {
        foreach (var action in primaryKeyBindings.Keys)
        {
            PlayerPrefs.SetInt(action + "_Primary", (int)primaryKeyBindings[action]);
        }

        foreach (var action in secondaryKeyBindings.Keys)
        {
            PlayerPrefs.SetInt(action + "_Secondary", (int)secondaryKeyBindings[action]);
        }

        PlayerPrefs.Save();
        optionsMenu?.UpdateKeyBindingTexts();
    }

    // Load keybindings from PlayerPrefs
    public void LoadKeybindings()
    {
        if (PlayerPrefs.HasKey("MoveLeft_Primary"))
        {
            SetPrimaryKey("MoveLeft", (KeyCode)System.Enum.ToObject(typeof(KeyCode), PlayerPrefs.GetInt("MoveLeft_Primary")),menu);
        }
        else
        {
            SetPrimaryKey("MoveLeft", KeyCode.A,menu); // Default
        }

        if (PlayerPrefs.HasKey("MoveLeft_Secondary"))
        {
            SetSecondaryKey("MoveLeft", (KeyCode)System.Enum.ToObject(typeof(KeyCode), PlayerPrefs.GetInt("MoveLeft_Secondary")),menu);
        }
        else
        {
            SetSecondaryKey("MoveLeft", KeyCode.LeftArrow,menu); // Default
        }

        if (PlayerPrefs.HasKey("MoveRight_Primary"))
        {
            SetPrimaryKey("MoveRight", (KeyCode)System.Enum.ToObject(typeof(KeyCode), PlayerPrefs.GetInt("MoveRight_Primary")),menu);
        }
        else
        {
            SetPrimaryKey("MoveRight", KeyCode.D,menu); // Default
        }

        if (PlayerPrefs.HasKey("MoveRight_Secondary"))
        {
            SetSecondaryKey("MoveRight", (KeyCode)System.Enum.ToObject(typeof(KeyCode), PlayerPrefs.GetInt("MoveRight_Secondary")),menu);
        }
        else
        {
            SetSecondaryKey("MoveRight", KeyCode.RightArrow,menu); // Default
        }

        if (PlayerPrefs.HasKey("Jump_Primary"))
        {
            SetPrimaryKey("Jump", (KeyCode)System.Enum.ToObject(typeof(KeyCode), PlayerPrefs.GetInt("Jump_Primary")),menu);
        }
        else
        {
            SetPrimaryKey("Jump", KeyCode.W,menu); // Default
        }

        if (PlayerPrefs.HasKey("Jump_Secondary"))
        {
            SetSecondaryKey("Jump", (KeyCode)System.Enum.ToObject(typeof(KeyCode), PlayerPrefs.GetInt("Jump_Secondary")),menu);
        }
        else
        {
            SetSecondaryKey("Jump", KeyCode.UpArrow,menu); // Default
        }

        Debug.Log("Keybindings have been reset to defaults.");

    }

public void ResetToDefaultKeybindings(OptionsMenu optionsMenu = null)
{
    // Reset MoveLeft
    SetPrimaryKey("MoveLeft", KeyCode.A,menu);
    SetSecondaryKey("MoveLeft", KeyCode.LeftArrow,menu);

    // Reset MoveRight
    SetPrimaryKey("MoveRight", KeyCode.D,menu);
    SetSecondaryKey("MoveRight", KeyCode.RightArrow,menu);

    // Reset Jump
    SetPrimaryKey("Jump", KeyCode.W,menu);
    SetSecondaryKey("Jump", KeyCode.UpArrow,menu);

    // Save the reset keybindings
    SaveKeybindings(menu);

    Debug.Log("Keybindings have been reset to defaults.");

    // Update the UI if a reference to OptionsMenu is provided
    optionsMenu?.UpdateKeyBindingTexts();
}

}

