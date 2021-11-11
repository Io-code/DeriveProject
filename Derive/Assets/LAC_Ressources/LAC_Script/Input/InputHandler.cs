using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public static class InputHandler
{
    public delegate void InteractDelegate(Controller controller);
    public static event InteractDelegate OnInteract, OnAttack;
    public static event Action<Vector2, Controller> OnMove;


    public static void CallInteract(Controller controller)
    {
        OnInteract?.Invoke(controller);
    }

    public static void CallAttack(Controller controller)
    {
        OnAttack?.Invoke(controller);
    }

    public static void CallMove(Vector2 dir, Controller controller)
    {
        OnMove?.Invoke(dir,controller);
    }


}
