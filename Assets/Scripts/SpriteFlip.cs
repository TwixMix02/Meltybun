using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlip : MonoBehaviour
{
    // The sprite renderer attached to the object
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer attached to the GameObject!");
        }
    }

    void Update()
    {
        // Check for input on the horizontal axis
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // If moving right, make the sprite face right
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        // If moving left, make the sprite face left
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}
