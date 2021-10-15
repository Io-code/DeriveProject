using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBehaviour : MonoBehaviour
{
    public GameObject bulletPrefab;

    [Range(0,360)]
    public float shootAngle; 
    public float shootInitialSpeed;
    

    [Header("Load")]

    public List<BulletBehaviour> bulletLoaded;
    public Vector3 loadPos;
    [Range(0.1f,2)]
    public float loadDetectRadius;
    GameObject loadObject;
    LoadPoint loadPoint;

    private void Awake()
    {
        loadObject = new GameObject("LoadDetector");
        loadObject.transform.parent = transform;
        loadObject.transform.position = loadPos;
        loadPoint =(LoadPoint)loadObject.AddComponent(typeof(LoadPoint));

        loadPoint.canon = this;
        loadPoint.radius = loadDetectRadius;
    }
    void CanonRotation( float angle)
    {
        shootAngle += angle;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, shootAngle );
    }

    void CanonShoot()
    {
        if(bulletLoaded.Count > 0)
        {
            Vector3 shootPos = new Vector3(Mathf.Cos(shootAngle * Mathf.Deg2Rad), Mathf.Sin(shootAngle * Mathf.Deg2Rad), 0);
            Vector2 shootDir = new Vector2(Mathf.Cos(shootAngle * Mathf.Deg2Rad), Mathf.Sin(shootAngle * Mathf.Deg2Rad));
            bulletLoaded[bulletLoaded.Count - 1].Shoot(shootPos, shootDir, shootInitialSpeed);
            bulletLoaded.Remove(bulletLoaded[bulletLoaded.Count - 1]);
        }
    }
    #region DEBUG
    [ContextMenu("CanonRotate")]
    public void DebugRotate()
    {
        CanonRotation(25);
    }

    [ContextMenu("CanonShoot")]
    public void DebugShoot()
    {
        CanonShoot();
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position  + new Vector3(Mathf.Cos(shootAngle * Mathf.Deg2Rad)  , Mathf.Sin(shootAngle * Mathf.Deg2Rad ),0), 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((loadObject)?loadObject.transform.position : transform.position + loadPos, loadDetectRadius);
    }

    
}
