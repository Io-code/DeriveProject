using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputHandler : MonoBehaviour
{
    public delegate void InteractDelegate(Controller controller);
    public event InteractDelegate OnInteract, OnAttack;
    public event Action<Vector2, Controller> OnMove;

    private static InputHandler instance = null;
    public static InputHandler Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this.gameObject);

    }
    public void CallInteract(Controller controller)
    {
        OnInteract?.Invoke(controller);
    }

    public void CallAttack(Controller controller)
    {
        OnAttack?.Invoke(controller);
    }

    public void CallMove(Vector2 dir, Controller controller)
    {
        OnMove?.Invoke(dir,controller);
    }
}
