using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boostPad : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 1.15f;
    [SerializeField] private float decelerationReduction = -1.05f; // Negative value to reduce deceleration
    [SerializeField] private float boostDuration = 5f;
    public Camera cam;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has a Character component
        Character character = other.GetComponent<Character>();
        
        if (character != null)
        {
            // Start the boost effect coroutine
            StartCoroutine(ApplyBoost(character));
        }
    }

    private IEnumerator ApplyBoost(Character character)
    {
        CameraBev bev = cam.GetComponent<CameraBev>();
        // Backup the original values
        float originalSpeed = character.topSpeed;
        float originalGravity = character.gravity;

        // Apply boost
        character.topSpeed *= speedMultiplier;
        character.gravity += decelerationReduction; // Reducing gravity to decrease deceleration
        CameraBev.boosted = true;

        // Wait for the boost duration
        yield return new WaitForSeconds(boostDuration);

        // Restore the original values
        character.topSpeed = originalSpeed;
        character.gravity = originalGravity;
        CameraBev.boosted = false;
    }
}

