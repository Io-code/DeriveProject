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

    public static InputHandler instance;
    public static InputHandler Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        UpdateSingleton();
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
    #region debug
    [ContextMenu("Update Singleton")]
    public void UpdateSingleton()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

    }
    #endregion

}
