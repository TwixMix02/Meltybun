using System;
using System.Collections;
using System.Data.Common;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathLoop : MonoBehaviour
{
    [SerializeField]public static int Aggression = 0;
    public static bool cameFromMenu = false;
    public static Character player;
    //public DeathClips dc;
    [SerializeField] private Camera mainCamera;
    private MusicPlayer mp;

    private static bool isInHerWorld = false;
    private static float startInHerWorldTime;
    public static bool isGO = false;
    public static bool isCom = false;

    public static bool needToGo = false;

    public static AudioSource audioSourceFalse;
    public AudioClip falsie;
    public AudioClip NHETheme;
    public static int currLevel = 1;

    [SerializeField] int currentLevel;

    public static int[] openSlots = {0,0,0}; //0 means open, 1 means closed.
    

    [SerializeField] public static float timeRate = 1f; // Multiplier for testing (default: real-time)

    private void Awake(){
        SceneManager.sceneLoaded += OnSceneLoaded;
        if(SceneManager.GetActiveScene().name == "Level 1"){
            currentLevel = 1;
            DeathLoop.currLevel = 1;
            Debug.Log("We are at LEVEL 1!!");
        }
        else if(SceneManager.GetActiveScene().name == "Level 2"){
            currentLevel = 2;
            DeathLoop.currLevel = 2;
            Debug.Log("We are at LEVEL 2!!");
        }
        else{
            currentLevel = 3;
            DeathLoop.currLevel = 3;
            Debug.Log("We are at LEVEL 3!!");
        }

        mp = FindObjectOfType<MusicPlayer>();
        mp.PlaySong("Song1");
    }
    private void Start()
    {
        //dc = GetComponent<DeathClips>();
        DeathLoop.findLevel();
        Aggression = 0;
        Debug.Log("Delta Time is:" + Time.deltaTime.ToString());
        isGO = false;
        isCom = false;
        player = FindObjectOfType<Character>();
        audioSourceFalse = FindObjectOfType<AudioSource>();

        audioSourceFalse.clip = falsie;
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

        //StartCoroutine(CheckTransportChance());
        StartCoroutine(IncreaseAggressionOverTime()); // Start the Aggression increment coroutine
    }

    public void Update(){
        DeathLoop.findLevel();
        currentLevel = FileManager.saveFile;
    }

    public void ToNHE()
    {
        // Fallback to Camera.main if mainCamera is null
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Get the main camera from the scene
            if (mainCamera == null)
            {
                Debug.LogError("No Main Camera found in the scene.");
                return;
            }
        }

        if (player == null)
        {
            Debug.LogError("Character instance is null in ToNHE.");
            return;
        }

        NHE.inHerWorld = true;
        isInHerWorld = true;
        startInHerWorldTime = Time.time;

        // Transport player and camera
        player.transform.position = new Vector3(player.transform.position.x, 672, player.transform.position.z);
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, 666, mainCamera.transform.position.z);

        // Trigger aggression decrease immediately
        AggressionMeter aggressionMeter = FindObjectOfType<AggressionMeter>();
        if (aggressionMeter != null)
        {
            aggressionMeter.StartDecrease(); // Trigger decrease on entering NHE
        }
        else
        {
            Debug.LogWarning("AggressionMeter instance not found in the scene.");
        }
        StopCoroutine(IncreaseAggressionOverTime());
        StartCoroutine(DecreaseAggressionOverTime());
        mp.StopSong();
        mp.PlaySong("Song2");
        Debug.Log("Player and camera have been transported to NHE's world.");
        StartCoroutine(CheckReturnToMainWorld());
    }

    public void toReturn(){
        StopCoroutine(DecreaseAggressionOverTime());
        StartCoroutine(IncreaseAggressionOverTime()); // Start the Aggression increment coroutine
        mp.StopSong();
        mp.PlaySong("Song1");
        ReturnToMainWorld();
    }
    public static void ReturnToMainWorld()
    {
        NHE.inHerWorld = false;
        isInHerWorld = false;

        player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z);
        Aggression = 0;

        Debug.Log("Player has been returned to the main world.");
    }
    public static void findLevel(){
        if(SceneManager.GetActiveScene().ToString() == "Level 1"){
            FileManager.saveFile = 1;
        }
        if(SceneManager.GetActiveScene().ToString() == "Level 2"){
            FileManager.saveFile = 2;
        }
        if(SceneManager.GetActiveScene().ToString() == "Level 3"){
            FileManager.saveFile = 3;
        }
    }
    // New coroutine to gradually increase Aggression
public IEnumerator IncreaseAggressionOverTime()
{
    AggressionMeter aggressionMeter = FindObjectOfType<AggressionMeter>();
    if (aggressionMeter != null){
        aggressionMeter.StartIncrease(); // Trigger decrease on entering NHE
    }
    else{    
        Debug.Log("INCREASE HAS BEGUN!");
    }

    // Define target values
    float targetAggression = 100f; // Maximum Aggression value
    float duration; // Duration in real seconds to reach max Aggression (based on real time)
    switch(currentLevel){
            case 1:
            duration = 300f;
            break;
            case 2:
            duration = 200f;
            break;
            case 3:
            duration  = 60f;
            break;
            default:
            duration  = 100f;
            break;
        }
    float elapsedTime = 0f;

    // Ensure Aggression starts at least at 1
    Aggression = Mathf.Max(Aggression, 1);

    // The increase in aggression over time will be based on timeRate
    while (Aggression < targetAggression)
    {
        // Increment elapsedTime by the scaled deltaTime (considering the timeRate)
        elapsedTime += Time.deltaTime * timeRate;
        //elapsedTime += 1f * timeRate;

        // Calculate the progression of aggression over the duration
        Aggression = Mathf.RoundToInt(Mathf.Lerp(1f, targetAggression, elapsedTime / duration));

        // Ensure aggression doesn't exceed the target
        if (Aggression >= targetAggression)
        {
            Aggression = Mathf.RoundToInt(targetAggression);
            Debug.Log("Aggression reached 100!");
            needToGo = true;
            if (needToGo)
            {
                ToNHE(); // Transport to NHE once aggression reaches 100
            }
            needToGo = false;
            yield break; // Exit the coroutine
        }

        // Log current aggression for debugging
        //Debug.Log("Current Aggression: " + Aggression);

        yield return null; // Wait for the next frame before updating again
    }
}

public IEnumerator DecreaseAggressionOverTime()
{
    // Define target values
    float targetAggression = 0f; // Minimum Aggression value (back to 0)
    float duration = 30f; // Duration in real seconds to reach minimum Aggression (set to 30 seconds)
    float elapsedTime = 0f;

    // Ensure Aggression starts at 100 if it's higher than that
    Aggression = Mathf.Min(Aggression, 100);

    // The decrease in aggression over time will be based on timeRate
    while (Aggression > targetAggression)
    {
        // Increment elapsedTime by the scaled deltaTime (considering the timeRate)
        elapsedTime += Time.deltaTime * timeRate;

        // Calculate the progression of aggression over the duration
        Aggression = Mathf.RoundToInt(Mathf.Lerp(100f, targetAggression, elapsedTime / duration));

        // Ensure aggression doesn't go below the target
        if (Aggression <= targetAggression)
        {
            Aggression = Mathf.RoundToInt(targetAggression);
            Debug.Log("Aggression reached 0!");
            needToGo = true;

            if (needToGo) // Only return to the main world if we're not already there
            {
                toReturn(); // Transport back to main world once aggression reaches 0
            }

            needToGo = false;

            yield break; // Exit the coroutine
        }

        // Log current aggression for debugging
        // Debug.Log("Current Aggression: " + Aggression);

        yield return null; // Wait for the next frame before updating again
    }
}


    private IEnumerator CheckTransportChance()
    {
        while (true)
        {
            int interval = Mathf.Max(1, 30 - 2 * Aggression);
            float scaledInterval = interval / timeRate; // Scale the interval by the timeRate

            yield return new WaitForSeconds(scaledInterval); // Wait time adjusted by timeRate

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

            if (UnityEngine.Random.Range(0f, 100f) < chance)
            {
                if (needToGo)
                {
                    ToNHE();
                }
                needToGo = false;
            }
        }
    }

    private static IEnumerator CheckReturnToMainWorld()
    {
        // Wait before starting return process
        float waitTime = 30f / timeRate; // Adjust the time to scale with timeRate
        yield return new WaitForSeconds(waitTime);

        while (isInHerWorld)
        {
            // Adjust time for each check based on timeRate
            yield return new WaitForSeconds(5f / timeRate);

            // If aggression reaches 0, return to the main world
            if (Aggression <= 0)
            {
                ReturnToMainWorld();
                needToGo = true;
                yield break; // Exit the coroutine once we've returned
            }
        }
    }



    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level 1")
        {
            if (cameFromMenu)
            {
                Aggression = 0;
                cameFromMenu = false;
            }
            else
            {
                Aggression++;
            }
        }
    }

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

    public static void ExecuteFalseDeath()
    {
        DeathLoop.PlayAS(DeathLoop.audioSourceFalse);  
        SetCameFromLevel();
        String test = SceneManager.GetActiveScene().name;
        Debug.Log("Testing: " + test);
        SceneManager.LoadScene(test);
        Menu.firstRun = false;
    }


    public static void PlayAS(AudioSource audioSource)
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned!");
            return;
        }

        // Play the audio clip
        audioSource.Play();
    }

    public static void ExecuteTrueDeath()
    {
        SetGameOver();
        SceneManager.LoadScene("Game End");
    }

    public void ExecuteLevelCompletion(int currLevel)
    {
        Debug.Log("LEVEL COMPLETE!");
        currLevel++;
        if(currLevel < 4){
        SavePlayerLevel(currLevel);
        loadLevel(currLevel);
        SetGameEnd();
        }
        else{
            SetGameEnd();
            SceneManager.LoadScene("Game End");
        }
    }

    // To save the player's level placement
    public void SavePlayerLevel(int level)
    {
        if(FileManager.saveFile <= 3){
            Debug.Log("#2:" + FileManager.saveFile.ToString());
            FileManager.SetFileName("Sav0"+(FileManager.saveFile-1).ToString()+".txt");
            FileManager.SaveData("CurrentLevel", level.ToString());
            FileManager.SaveFloat("TimeRemaining", 600-TimerManager.currentTime);
            FileManager.Flush();
        }
    }

    public void loadLevel(int level){
        switch(level){
            case 2:
            SceneManager.LoadScene("Level 2");
            break;
            case 3:
            SceneManager.LoadScene("Level 3");
            break;
            case 4:
            SceneManager.LoadScene("Game End");
            break;
            default:
            Debug.Log("You have no save data here! Try again!");
            break;
        }
    }
}
