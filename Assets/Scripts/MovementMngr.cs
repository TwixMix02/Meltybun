using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMngr : MonoBehaviour
{
    [SerializeField] Character snoc;
    private InputHandler inputHandler;

    void Start()
    {
        snoc = GetComponent<Character>();
        snoc.currSpeed = 0;
        snoc.topSpeed = 10;

        // Create and bind commands
        inputHandler = new InputHandler();
        inputHandler.BindCommand("MoveLeft", new MoveLeftCommand(snoc, snoc.topSpeed));
        inputHandler.BindCommand("MoveRight", new MoveRightCommand(snoc, snoc.topSpeed));
        inputHandler.BindCommand("Jump", new JumpCommand(snoc));
    }

    void FixedUpdate()
    {
        inputHandler.HandleInput();

        // Reset movement flag if no key is pressed
        if (!Input.anyKey)
        {
            snoc.isMoving = false;
        }
    }
}
