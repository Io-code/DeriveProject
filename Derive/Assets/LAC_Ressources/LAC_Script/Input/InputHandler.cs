using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    
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
        if (instance = null)
            instance = this;
        else
            Destroy(this.gameObject);

    }
}
