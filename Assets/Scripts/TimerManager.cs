using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText; // Assign this in the Inspector
    [SerializeField] private float maxTime = 600f; // 10 minutes in seconds
    public static float currentTime = 0f; // Start from 0

    private bool timerRunning = true;

    private void Start()
    {
        UpdateTimerUI();
    }

    private void Awake(){
        currentTime = 0f;
    }

    private void Update()
    {
        if (timerRunning)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= maxTime)
            {
                currentTime = maxTime;
                timerRunning = false;
                RestartGame();
            }

            UpdateTimerUI();
        }
    }

    private void UpdateTimerUI()
    {
        // Convert time to minutes:seconds format
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // Update the timer text
        timerText.text = $"TIMER: {minutes:00}:{seconds:00}";
    }

    private void RestartGame()
    {
        Debug.Log("Timer reached 10 minutes. Restarting the game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
