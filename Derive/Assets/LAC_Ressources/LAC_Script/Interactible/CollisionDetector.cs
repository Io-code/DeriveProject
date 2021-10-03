using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CircleCollider2D))]
public class CollisionDetector : MonoBehaviour
{
    CircleCollider2D cC2D;
    public enum CollisionType { PLAYER, OTHER};
    public CollisionType collisionType;
    public Action< GameObject> OnCollision;

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
                        OnCollision?.Invoke(collision.gameObject);

                    break;
                }
        }
    }
}
