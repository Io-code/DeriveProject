using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputHandler : MonoBehaviour
{
    public delegate void InteractDelegate(Controller controller);
    public event InteractDelegate OnInteract, OnAttack;
    public event Action<Vector2> OnMove;

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
    public void PerformInteract(Controller controller)
    {
        OnInteract?.Invoke(controller);
    }

    public void PerformAttack(Controller controller)
    {
        OnAttack?.Invoke(controller);
    }

    public void PerformMove(InputAction.CallbackContext value)
    {
        OnMove?.Invoke( value.ReadValue<Vector2>());
        Debug.Log("Move : " + value.ReadValue<Vector2>());
    }
}
