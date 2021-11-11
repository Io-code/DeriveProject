using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Undestroy : MonoBehaviour
{
    GameObject[] game;
    private void Awake()
    {
        game = GameObject.FindGameObjectsWithTag("Uduino");
        if (game.Length == 1)
            DontDestroyOnLoad(gameObject);
        else DestroyObject(gameObject);
    }
}

