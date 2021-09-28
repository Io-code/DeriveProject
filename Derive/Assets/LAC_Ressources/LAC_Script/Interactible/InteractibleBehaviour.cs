using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CircleCollider2D))]
public class InteractibleBehaviour : MonoBehaviour
{
    public string detectTag;
    List<Controller> players = new List<Controller>();
    public event Action<Controller> InteractHappens;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        InputHandler.Instance.OnInteract += VerifyInteract;
    }
    void OnDisable()
    {
        InputHandler.Instance.OnInteract -= VerifyInteract;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == detectTag)
        {
            Controller currentPlayer = collision.GetComponent<Controller>();

            if (players.Contains(currentPlayer))
                players.Add(currentPlayer);
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == detectTag)
        {
            Controller currentPlayer = collision.GetComponent<Controller>();

            if (players.Contains(currentPlayer))
                players.Remove(currentPlayer);
        }
    }

    public void VerifyInteract(Controller controller)
    {
        if (players.Count == 1)
            InteractHappens?.Invoke(controller);
    }
}
