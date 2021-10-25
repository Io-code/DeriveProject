using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class LoadPoint : MonoBehaviour
{
    public CanonBehaviour canon;
    public float radius;
    CircleCollider2D cC2D;

    private void Start()
    {
        cC2D = GetComponent<CircleCollider2D>();
        cC2D.isTrigger = true;
        cC2D.radius = radius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            BulletBehaviour bullet = collision.GetComponent<BulletBehaviour>();
            if (!canon.bulletLoaded.Contains(bullet))
                canon.bulletLoaded.Add(bullet);

            bullet.Load();
        }
    }
}
