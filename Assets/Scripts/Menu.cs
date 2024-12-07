using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public static bool firstRun = false;
    public GameObject optionsPanel;
    public GameObject keyBindsPanel;

    public GameObject loadPanel;

    public void Awake(){
        FileManager.FileCheck();
        DeathLoop.currLevel = 1;
    }

    // Method to start the game, either for the first run or to restart
    public void StartGame(bool isFirstRun = false)
    {
        DeathLoop.SetCameFromMenu();  // Signal that we’re coming from Menu
        SceneManager.LoadScene("Level 1");
        Debug.Log("#3:" + FileManager.saveFile.ToString());
        if (isFirstRun) firstRun = true;
    }

    // Method to quit the game
    public void ExitGame()
    {
        Application.Quit();
    }

    // Toggles visibility of the options panel
    public void ToggleOptionsPanel()
    {
        TogglePanel(optionsPanel);
    }

    // Toggles visibility of the keybinding panel
    public void ToggleKeybindsPanel()
    {
        TogglePanel(keyBindsPanel);
    }

     public void ToggleLoadPanel()
    {
        TogglePanel(loadPanel);
    }

    // General method to toggle panel visibility
    private void TogglePanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf);
        }
        else
        {
            Debug.LogError($"{panel.name} reference not set!");
        }
    }

    /*
    public void loadFromSave(int levNum, string Name){
        DeathLoop.setPlayer(Name);
        switch(levNum){
        case 1:
            SceneManager.LoadScene("Level 1");
            break;
        case 2:
            SceneManager.LoadScene("Level 2");
            break;
        case 3:
            SceneManager.LoadScene("Level 3");
            break;
        default:
            SceneManager.LoadScene("Level 1");
            break;
        }
    }
    */

    public int LoadPlayerLevel()
    {
        // Default value if no level is saved yet
        int savedLevel = PlayerPrefs.GetInt("PlayerLevel", 0); // Default is 0
        Debug.Log("Player level loaded: " + savedLevel);
        return savedLevel;
    }
}
