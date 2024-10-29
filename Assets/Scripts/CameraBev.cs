using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBev : MonoBehaviour
{
    public Character snoc;
    [SerializeField] public float smoothSpeed = 1f;
    [SerializeField] public Vector3 offset;
    public Rigidbody2D rb;
    public static bool isJumping = false;

    void Start()
    {
        rb = snoc.GetComponent<Rigidbody2D>();
        offset = new Vector3(0.3f, 0, 0);
    }

    void LateUpdate()
    {
        Vector3 playerPosition = snoc.transform.position;
        Vector3 desiredPosition = playerPosition + offset;

        float leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 0)).x;

        // If the character is jumping, smoothly follow their Y position
        if (isJumping)
        {
            // Adjust both X and Y positions
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
        else
        {
            // Only adjust X position when not jumping
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.fixedDeltaTime);
            transform.position = new Vector3(smoothedPosition.x, transform.position.y, transform.position.z);
        }
    }
}

