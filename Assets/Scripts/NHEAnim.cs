using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NHEController : MonoBehaviour
{
    private Animator animator;
    private Vector3 previousPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        previousPosition = transform.position;
    }

    void Update()
    {
        // Check if the GameObject is moving
        bool isMoving = (transform.position != previousPosition);

        // Update the Animator parameter
        animator.SetBool("isMoving", isMoving);

        // Update the previous position
        previousPosition = transform.position;
    }
}

