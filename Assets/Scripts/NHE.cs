using System.Collections;
using UnityEngine;

public class NHE : MonoBehaviour
{
    public static bool inHerWorld = false;
    public static bool doesSheSee = false;
    public Character character; // Single Character instance for both actions
    private float currentSpeed = 0f;
    private float baseSpeed = 2.5f; //Starting speed.
    private float speedIncreaseRate = 0.1f; //Rate at which speed increases.
    private float maxSpeed = 10f;
    AudioSource aS;

    // Moves the character toward a target position at a specified speed
    public void MoveTowards(Vector3 targetPosition, float speed)
    {
        // Move towards the target position based on the current speed
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);
    }

  private void Start()
{
    aS = GetComponent<AudioSource>();
    if (aS == null)
    {
        Debug.LogError("No AudioSource found on this GameObject! Please add one.");
    }
    inHerWorld = false;
    doesSheSee = false;
    StartCoroutine(ChasePlayer());
}

    private IEnumerator ChasePlayer(){
        float elapsedTime = 0f; // Track time elapsed in `inHerWorld`

        while (true)
        {
            if (inHerWorld)
            {
                elapsedTime += Time.deltaTime;
                currentSpeed = Mathf.Min(baseSpeed + elapsedTime * speedIncreaseRate, maxSpeed);

                // Move the character toward the player's position
                MoveTowards(character.PlayerPosition, currentSpeed * Time.deltaTime);

                yield return null; // Wait until the next frame
            }
            else
            {
                elapsedTime = 0f; // Reset elapsed time when `inHerWorld` is false
                currentSpeed = baseSpeed;
                yield return null;
            }
        }
    }


    // Example method to retrieve the current level
    private int GetCurrentLevel()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Example mapping based on scene names
        if (sceneName.Contains("Level 1"))
            return 1;
        if (sceneName.Contains("Level 2"))
            return 2;
        if (sceneName.Contains("Level 3"))
            return 3;

        return 1; // Default to Level 1 if no match
    }



public void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Snocc"))
    {
        if (aS != null && aS.clip != null)
        {
            aS.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing. Check your setup.");
        }
    }
}


    
}
