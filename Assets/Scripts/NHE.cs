using System.Collections;
using UnityEngine;

public class NHE : MonoBehaviour
{
    public static bool inHerWorld = false;
    public static bool doesSheSee = false;
    public Character character; // Single Character instance for both actions
    public float maxSpeed = 25f;
    public float currentSpeed = 0f;

    // Moves the character toward a target position at a specified speed
    public void MoveTowards(Vector3 targetPosition, float speed)
    {
        // Move towards the target position based on the current speed
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);
    }

    private void Start()
    {
        inHerWorld = false;
        doesSheSee = false;
        StartCoroutine(ChasePlayer());
    }

    private IEnumerator ChasePlayer(){
        while (true)
        {
            if (inHerWorld)
            {
                Vector3 directionToPlayer = (character.PlayerPosition - character.transform.position).normalized;

                if (!doesSheSee)
                {
                    // Calculate speed based on aggression, with max speed at 25 units per second
                    currentSpeed = Mathf.Lerp(0, maxSpeed, DeathLoop.Aggression / 10f);
                }
                else
                {
                    // Follow at speed equal to Aggression + 1
                    currentSpeed = DeathLoop.Aggression + 1;
                }

                // Move the character toward the player's position
                MoveTowards(character.PlayerPosition, currentSpeed * Time.deltaTime);

                // Optional: Apply aerial movement if needed

                yield return null; // Wait until the next frame
            }
            else
            {
                yield return null;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Snocc")){
            DeathLoop.ExecuteTrueDeath();
        }
    }
}

