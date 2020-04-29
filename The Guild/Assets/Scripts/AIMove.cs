using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIMove : MonoBehaviour
{

    /*
    Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
    Converted to C# 27-02-13 - no credit wanted.
    Simple flycam I made, since I couldn't find any others made public.  
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    shift : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/

    enum MOVE_TYPE
    {
        HOLD_POSITION,
        FACE_NORTH,
        FACE_EAST,
        FACE_SOUTH,
        FACE_WEST,
        MOVE_FORAWRD,
        MOVE_BACK,
        MOVE_LEFT,
        MOVE_RIGHT,
        SPRINT,
        JUMP
    };


    float mainSpeed = 100.0f; //regular speed
    float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 1000.0f; //Maximum speed when running
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;

    private Queue driverCommands = new Queue();

    void PopulateCommandList()
    {
        driverCommands.Enqueue(MOVE_TYPE.MOVE_FORAWRD);
        driverCommands.Enqueue(MOVE_TYPE.MOVE_BACK);
        driverCommands.Enqueue(MOVE_TYPE.MOVE_LEFT);
        driverCommands.Enqueue(MOVE_TYPE.MOVE_RIGHT);        
    }

    int DriveInput()
    {
        if (driverCommands.Count != 0)
            return (int)driverCommands.Dequeue();
        else
            return 0; //returns the "hold position command"
    }

    void Update()
    {
        int moveCommand = DriveInput();

        //Keyboard commands
        float f = 0.0f;
        Vector3 p = GetBaseInput(moveCommand);
        if (moveCommand == (int)MOVE_TYPE.SPRINT)
        {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (moveCommand == (int)MOVE_TYPE.JUMP)
        { //If player wants to move on X and Z axis only
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            transform.Translate(p);
        }

    }

    private Vector3 GetBaseInput(int command)
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (command == (int)MOVE_TYPE.MOVE_FORAWRD)
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (command == (int)MOVE_TYPE.MOVE_BACK)
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (command == (int)MOVE_TYPE.MOVE_LEFT)
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (command == (int)MOVE_TYPE.MOVE_RIGHT)
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }

}