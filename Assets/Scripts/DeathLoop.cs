using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathLoop : MonoBehaviour
{
    public static int Aggression = 0;
    public static bool cameFromMenu = false;
    public static Character player;
    [SerializeField] private Camera mainCamera;

    private static bool isInHerWorld = false;
    private static float startInHerWorldTime;
    public static bool isGO = false;
    public static bool isCom = false;

    private void Start()
    {
        isGO = false;
        isCom = false;
        player = FindObjectOfType<Character>();

        if (player == null)
        {
            Debug.LogError("Character instance not found in the scene.");
            return;
        }

        if (mainCamera == null)
        {
            Debug.LogError("Camera instance not assigned in the Inspector.");
            return;
        }

        StartCoroutine(CheckTransportChance());
    }

    public void ToNHE() // Changed to an instance method
    {
        if (player == null || mainCamera == null)
        {
            Debug.LogError("Character or Camera instance is null in ToNHE.");
            return;
        }

        NHE.inHerWorld = true;
        isInHerWorld = true;
        startInHerWorldTime = Time.time;

        player.transform.position = new Vector3(player.transform.position.x, 672, player.transform.position.z);
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, 666, mainCamera.transform.position.z);

        Debug.Log("Player and camera have been transported to NHE's world.");
        StartCoroutine(CheckReturnToMainWorld()); // Called directly on the instance
    }

    // Coroutine to check periodically if the player should be transported
    private IEnumerator CheckTransportChance()
    {
        while (true)
        {
            // Calculate interval based on Aggression level
            int interval = Mathf.Max(1, 30 - 2 * Aggression);
            yield return new WaitForSeconds(interval);

            // Calculate the chance of transport based on Aggression
            float chance;
            if (Aggression == 9)
            {
                chance = 95f;
            }
            else if (Aggression >= 0 && Aggression < 8)
            {
                chance = 100f - (100f - 10f * (Aggression + 1));
            }
            else
            {
                chance = 99f; // Aggression == 10 or above
            }

            // Roll for transport based on calculated chance
            if (Random.Range(0f, 100f) < chance)
            {
                ToNHE(); // Trigger transport
            }
        }
    }

    // Coroutine to check if player should be returned to the main world
    private static IEnumerator CheckReturnToMainWorld()
    {
        // Wait until player has been in NHE's world for at least 30 seconds
        yield return new WaitForSeconds(30);

        while (isInHerWorld)
        {
            yield return new WaitForSeconds(5);

            // 1/10 chance to return to the main world
            if (Random.Range(0, 10) == 0)
            {
                ReturnToMainWorld();
            }
        }
    }

    // Function to return the player to the main world
    public static void ReturnToMainWorld()
    {
        NHE.inHerWorld = false;
        isInHerWorld = false;

        // Return player and camera to the main world's position (e.g., y = 0)
        player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z);

        Debug.Log("Player has been returned to the main world.");
    }



    void Awake()
    {
        // Register the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Unregister event to avoid memory leaks
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Function to handle scene loading logic
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if we're entering Level 1
        if (scene.name == "Level 1")
        {
            if (cameFromMenu)
            {
                Aggression = 0;  // Reset aggression if came from menu
                cameFromMenu = false;  // Reset the flag for subsequent restarts
            }
            else
            {
                Aggression++;  // Increment aggression if this is a restart
            }
        }
    }

    // Method to set the cameFromMenu flag, called from Menu.cs
    public static void SetCameFromMenu()
    {
        cameFromMenu = true;
    }

    public static void SetCameFromLevel()
    {
        cameFromMenu = false;
    }

    public static void SetGameOver()
    {
        isGO = true;
    }

    public static void SetGameEnd()
    {
        isCom = true;
    }

    public int GetAggression()
    {
        return Aggression;
    }

    // Testing method to restart level and increment Aggression if not from menu
    public static void ExecuteFalseDeath()
    {
        SetCameFromLevel();
        SceneManager.LoadScene("Level 1");
        Menu.firstRun = false;
    }

    public static void ExecuteTrueDeath()
    {
        SetGameOver();
        SceneManager.LoadScene("Demo End");
        
        
    }

    public static void ExecuteVictory()
    {
        SetGameEnd();
        SceneManager.LoadScene("Demo End");
    }
    

    void Update()
    {
        //Debug.Log("Aggression = " + GetAggression().ToString());
    }
}
