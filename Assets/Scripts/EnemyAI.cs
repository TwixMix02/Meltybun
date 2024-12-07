using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrolling")]
    public float patrolSpeed = 2f;
    public float patrolDistance = 5f;

    [Header("Chasing")]
    public float chaseSpeed = 4f;
    public float detectionRadius = 5f;

    private Vector3 startingPosition;
    private bool movingRight = true;
    private bool isChasing = false;

    private Character targetCharacter;

    private void Start()
    {
        // Store the initial position for patrol logic
        startingPosition = transform.position;

        // Find the first GameObject with a Character component
        targetCharacter = FindObjectOfType<Character>();
        if (targetCharacter == null)
        {
            Debug.LogError("No GameObject with Character component found in the scene.");
        }
    }

    private void Update()
    {
        if (targetCharacter == null) return;

        if (isChasing)
        {
            ChaseCharacter();
        }
        else
        {
            Patrol();
            DetectCharacter();
        }
    }

    private void Patrol()
    {
        float patrolLimit = patrolDistance / 2;

        if (movingRight)
        {
            transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
            if (transform.position.x > startingPosition.x + patrolLimit)
                movingRight = false;
        }
        else
        {
            transform.Translate(Vector2.left * patrolSpeed * Time.deltaTime);
            if (transform.position.x < startingPosition.x - patrolLimit)
                movingRight = true;
        }
    }

    private void DetectCharacter()
    {
        // Check if the character is within a 5x5 radius
        Vector2 distanceToCharacter = targetCharacter.transform.position - transform.position;
        if (Mathf.Abs(distanceToCharacter.x) <= detectionRadius && Mathf.Abs(distanceToCharacter.y) <= detectionRadius)
        {
            PlayWithRandomPitch();
            isChasing = true;
        }
    }

    private void ChaseCharacter()
    {
        if (targetCharacter == null) return;

        // Move towards the character's position
        Vector2 direction = (targetCharacter.transform.position - transform.position).normalized;
        transform.Translate(direction * chaseSpeed * Time.deltaTime);

        // Stop chasing if the character leaves the detection radius
        Vector2 distanceToCharacter = targetCharacter.transform.position - transform.position;
        if (Mathf.Abs(distanceToCharacter.x) > detectionRadius || Mathf.Abs(distanceToCharacter.y) > detectionRadius)
        {
            isChasing = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the detection radius in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(detectionRadius * 2, detectionRadius * 2, 0));
    }

        void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Snocc")){
                
                DeathLoop.ExecuteFalseDeath();
        }
        else{
            return;
        }
    }

    // Reference to the AudioSource component
    public AudioSource audioSource;

    public void PlayWithRandomPitch()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned!");
            return;
        }

        // Set pitch to normal (1) or inverted (-1) with a 50% chance
        audioSource.pitch = Random.value > 0.5f ? 1f : -1f;

        // Play the audio clip
        audioSource.Play();
    }
}

