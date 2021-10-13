using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public int MaxDamage;
    int currentDamage;

    float damageDelay = 0.2f;
    bool canTakeDamage = true;

    private void Awake()
    {
        currentDamage = MaxDamage;
    }
    public void TakeDamage( int damage)
    {
        if (canTakeDamage)
        {
            canTakeDamage = false;

            currentDamage -= damage;
            if (currentDamage <= 0)
                DestroyWall();

            StartCoroutine(DamageCooldown());
        }
        
        
    }

    void DestroyWall()
    {
        Destroy(gameObject);
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageDelay);
        canTakeDamage = true;
    }
}
