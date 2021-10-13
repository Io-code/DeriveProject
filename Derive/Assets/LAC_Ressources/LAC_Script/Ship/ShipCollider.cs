using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollider : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("exit player " + collision.name);
            ShipEvent.PerformExitPlayer(collision.gameObject);
        }
            

        if (collision.GetComponent<ThrowBehaviour>() != null)
            ShipEvent.PerformExitObj(collision.gameObject);
    }
}
