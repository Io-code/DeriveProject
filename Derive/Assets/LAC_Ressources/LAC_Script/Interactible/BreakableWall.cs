using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public int MaxDamage;
    public int currentDamage = 0;

    public MeshRenderer mesh;
    public Material[] changeMat;

    float damageDelay = 0.2f;
    bool canTakeDamage = true;


    public void TakeDamage( int damage)
    {
        if (canTakeDamage)
        {
            canTakeDamage = false;

            currentDamage += damage;
            if (currentDamage >= MaxDamage)
                DestroyWall();

            mesh.material = changeMat[Mathf.Clamp(currentDamage, 0, changeMat.Length)];

            StartCoroutine(DamageCooldown());
            Debug.Log("Take damage");
        }
        
        
    }

    void DestroyWall()
    {
        Destroy(transform.parent.gameObject); 
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageDelay);
        canTakeDamage = true;
    }
}
