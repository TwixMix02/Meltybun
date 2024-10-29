using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public static bool firstRun = false;

    public void startExecutable()
    {
        DeathLoop.SetCameFromMenu();  // Signal that we’re coming from Menu
        SceneManager.LoadScene("Level 1");
        firstRun = true;
    }

    public void exitExecutable()
    {
        Application.Quit();
    }

    public void startAgain(){
        DeathLoop.SetCameFromMenu();  // Signal that we’re coming from Menu
        SceneManager.LoadScene("Level 1");
    }

}