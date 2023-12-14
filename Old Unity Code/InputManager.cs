using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static bool AttackButtonDown;
    public static bool UseButtonDown;
    public static bool PauseButtonDown;

    public static bool MoveUpButton;
    public static bool MoveDownButton;
    public static bool MoveLeftButton;
    public static bool MoveRightButton;
    public static Vector2 MoveAxis;

    public static KeyCode AttackKey = KeyCode.Mouse0;
    public static KeyCode UseKey = KeyCode.Mouse1;
    public static KeyCode PauseKey = KeyCode.Escape;

    public static KeyCode MoveUpKey = KeyCode.W;
    public static KeyCode MoveDownKey = KeyCode.S;
    public static KeyCode MoveLeftKey = KeyCode.A;
    public static KeyCode MoveRightKey = KeyCode.D;

    public static List<KeyCode> DownKeys = new List<KeyCode>();
    void Update()
    {
        if (Input.GetKeyDown(AttackKey))
        {
            AttackButtonDown = true;
        }
        else
        {
            AttackButtonDown = false;
        }
        if (Input.GetKeyDown(UseKey))
        {
            UseButtonDown = true;
        }
        else
        {
            UseButtonDown = false;
        }
        if (Input.GetKeyDown(PauseKey))
        {
            PauseButtonDown = true;
        }
        else
        {
            PauseButtonDown = false;
        }
        if (Input.GetKey(MoveDownKey))
        {
            MoveDownButton = true;
        }
        else
        {
            MoveDownButton = false;
        }
        if (Input.GetKey(MoveUpKey))
        {
            MoveUpButton = true;
        }
        else
        {
            MoveUpButton = false;
        }
        if (Input.GetKey(MoveLeftKey))
        {
            MoveLeftButton = true;
        }
        else
        {
            MoveLeftButton = false;
        }
        if (Input.GetKey(MoveRightKey))
        {
            MoveRightButton = true;
        }
        else
        {
            MoveRightButton = false;
        }

        if (MoveUpButton)
        {
            MoveAxis.y = 1;
        }
        else if (MoveDownButton)
        {
            MoveAxis.y = -1;
        }
        else
        {
            MoveAxis.y = 0;
        }
        if (MoveRightButton)
        {
            MoveAxis.x = 1;
        }
        else if (MoveLeftButton)
        {
            MoveAxis.x = -1;
        }
        else
        {
            MoveAxis.x = 0;
        }
    }
}
