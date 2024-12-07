using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base command interface
public interface ICommand
{
    void Execute();
}

// Concrete command for moving left
public class MoveLeftCommand : ICommand
{
    private Character character;
    private float speed;

    public MoveLeftCommand(Character character, float speed)
    {
        this.character = character;
        this.speed = speed;
    }

    public void Execute()
    {
        Vector3 moveVector = new Vector3(-1, 0, 0);
        character.isMoving = true;
        character.currSpeed = character.speedUpdate(character.currSpeed);
        character.transform.localPosition += moveVector * character.currSpeed * Time.fixedDeltaTime;
    }
}

// Concrete command for moving right
public class MoveRightCommand : ICommand
{
    private Character character;
    private float speed;

    public MoveRightCommand(Character character, float speed)
    {
        this.character = character;
        this.speed = speed;
    }

    public void Execute()
    {
        Vector3 moveVector = new Vector3(1, 0, 0);
        character.isMoving = true;
        character.currSpeed = character.speedUpdate(character.currSpeed);
        character.transform.localPosition += moveVector * character.currSpeed * Time.fixedDeltaTime;
    }
}

// Concrete command for jumping
public class JumpCommand : ICommand
{
    private Character character;

    public JumpCommand(Character character)
    {
        this.character = character;
    }

    public void Execute()
    {
        character.jump();
    }
}

