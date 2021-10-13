using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CircleCollider2D))]
public class InteractibleBehaviour : MonoBehaviour
{
    public string detectTag;

    [Range(0,5)]
    public float detectRadius;
    public CircleCollider2D cC2D;

    public List<Controller> players = new List<Controller>();
    public event Action<Controller> InteractHappens;
    public bool oneController;

    // Start is called before the first frame update
    public void Awake()
    {
        cC2D.radius = detectRadius;
    }
    void OnEnable()
    {
        InputHandler.instance.OnInteract += VerifyInteract;
    }
    void OnDisable()
    {
        InputHandler.instance.OnInteract -= VerifyInteract;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == detectTag)
        {
            Controller currentPlayer = collision.GetComponent<Controller>();
            if (!players.Contains(currentPlayer))
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
        if ((players.Count == 1 && controller == players[0]) || (!oneController && players.Count > 0))
        {
            Debug.Log("Verifiy : " + players[0]);
            InteractHappens?.Invoke(controller);
        }
            
    }

    private void OnDrawGizmos()
    {
        if (players.Count > 0)
            Gizmos.color = (players.Count == 1 || !oneController) ? Color.green : Color.red;
        else
            Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
