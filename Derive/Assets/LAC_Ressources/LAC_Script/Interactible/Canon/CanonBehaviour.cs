using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBehaviour : MonoBehaviour
{

    public ReadEncoder encoder;
    public float RotationSpeed;
    [Range(0,360)]
    public float shootAngle;
    [Range(0,5)]
    public float shootDist;
    public float zOffset;
    public float shootInitialSpeed;
    
    

    [Header("Load")]

    public List<BulletBehaviour> bulletLoaded;
    public Vector3 loadPos;
    [Range(0.1f,5)]
    public float loadDetectRadius;
    GameObject loadObject;
    LoadPoint loadPoint;

    [Header("Shoot")]
    public List<GameObject> playerObj = new List<GameObject>(2);
    public CollisionDetector colDetect;
    public List<Controller> controllers = new List<Controller>();



    private void OnEnable()
    {
        InputHandler.OnAttack += CanonShoot;
        colDetect.OnCollisionPlayer += StartController;
        colDetect.OnCollisionExitPlayer += EndController;
    }
    private void OnDisable()
    {
        colDetect.OnCollisionPlayer -= StartController;
        colDetect.OnCollisionExitPlayer -= EndController;
        InputHandler.OnAttack -= CanonShoot;
    }
    private void Awake()
    {
        loadObject = new GameObject("LoadDetector");
        encoder = GameObject.Find("Uduino").GetComponent<ReadEncoder>();
        //loadObject.transform.position = transform.position;
        loadObject.transform.position = transform.position + loadPos;
        loadObject.transform.parent = transform;
        loadPoint =(LoadPoint)loadObject.AddComponent(typeof(LoadPoint));

        loadPoint.canon = this;
        loadPoint.radius = loadDetectRadius;
    }

    private void Update()
    {
        if(controllers.Count > 0)
        {
            CanonRotation(RotationSpeed * encoder.UpdateRead(controllers[0].gameObject == playerObj[0]));
        }
    }

    void StartController( GameObject obj)
    {
        if(!controllers.Contains(obj.GetComponent<Controller>()))
        {
            Debug.Log("Start Controll Canon");
            controllers.Add(obj.GetComponent<Controller>());
            bool firstPlayer = obj == playerObj[0];
            encoder.StartRead(firstPlayer);
        }

    }

    void EndController(GameObject obj)
    {
        if (controllers.Contains(obj.GetComponent<Controller>()))
        {
            Debug.Log("End Controll Canon");
            controllers.Remove(obj.GetComponent<Controller>());
        }
    }
    void CanonRotation( float angle)
    {
        shootAngle += angle;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, shootAngle );
    }


    void CanonShoot(Controller ctrl)
    {
        if (controllers.Count == 0)
            return;

        if(ctrl == controllers[0])
        {
            Debug.Log("Fire");
            if (bulletLoaded.Count > 0)
            {
                Vector3 shootPos = new Vector3(Mathf.Cos(shootAngle * Mathf.Deg2Rad) * shootDist, Mathf.Sin(shootAngle * Mathf.Deg2Rad) * shootDist, zOffset);
                Vector2 shootDir = new Vector2(Mathf.Cos(shootAngle * Mathf.Deg2Rad), Mathf.Sin(shootAngle * Mathf.Deg2Rad));


                //bulletLoaded[bulletLoaded.Count - 1].Shoot(shootPos + transform.position, shootDir, shootInitialSpeed);
                bulletLoaded[0].transform.position = shootPos + transform.position;
                bulletLoaded[0].Throw(shootDir, shootInitialSpeed);

                bulletLoaded.Remove(bulletLoaded[0]);
            }
        }
    }
    #region DEBUG
    [ContextMenu("CanonRotate")]
    public void DebugRotate()
    {
        CanonRotation(25 );
    }

    [ContextMenu("CanonShoot")]
    public void DebugShoot()
    {
        //CanonShoot(null);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position + new Vector3(Mathf.Cos(shootAngle * Mathf.Deg2Rad) * shootDist, Mathf.Sin(shootAngle * Mathf.Deg2Rad) * shootDist, zOffset), 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((loadObject)?loadObject.transform.position : transform.position + loadPos, loadDetectRadius);

        Vector2 shootDir = new Vector2(Mathf.Cos(shootAngle * Mathf.Deg2Rad), Mathf.Sin(shootAngle * Mathf.Deg2Rad));
        Debug.DrawRay(transform.position + new Vector3(Mathf.Cos(shootAngle * Mathf.Deg2Rad) * shootDist, Mathf.Sin(shootAngle * Mathf.Deg2Rad) * shootDist, zOffset), shootDir * 1.5f);
    }
}
