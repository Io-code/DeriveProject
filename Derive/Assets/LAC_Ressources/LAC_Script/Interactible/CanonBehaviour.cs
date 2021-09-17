using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBehaviour : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Vector3 firePoint;

    [Range(0,360)]
    public float shootAngle; 
    public float shootInitialSpeed;

    public bool load;
    void CanonRotation( float angle)
    {
        shootAngle += angle;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, shootAngle );
    }

    #region DEBUG
    [ContextMenu("CanonRotate")]
    public void DebugRotate()
    {
        CanonRotation(25);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + firePoint + new Vector3(Mathf.Cos(shootAngle * Mathf.Deg2Rad), Mathf.Sin(shootAngle * Mathf.Deg2Rad),0), 0.5f);
    }
}
