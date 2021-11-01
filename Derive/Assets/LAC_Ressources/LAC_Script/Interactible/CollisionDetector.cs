using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CircleCollider2D))]
public class CollisionDetector : MonoBehaviour
{
    [HideInInspector]
    public CircleCollider2D cC2D;
    public enum CollisionType { PLAYER, OTHER};
    public CollisionType collisionType;
    public Action< GameObject> OnCollisionPlayer, OnCollisionWall;

    private void Awake()
    {
        cC2D = GetComponent<CircleCollider2D>();
        cC2D.isTrigger = true;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collisionType)
        {
            case CollisionType.PLAYER:
                {
                    if (collision.tag == "Player")
                    {
                        OnCollisionPlayer?.Invoke(collision.gameObject);
                        Debug.Log("HitPlayer");
                    }
                       

                    break;
                }
        }

        if(collision.tag == "BreakableWall")
        {
            Debug.Log("Hit wall");
            BreakableWall wall = collision.GetComponent<BreakableWall>();
            wall.TakeDamage(1);
        }
            
        
    }
}
