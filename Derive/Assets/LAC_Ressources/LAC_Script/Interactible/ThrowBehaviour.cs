using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class ThrowBehaviour : MonoBehaviour
{
    public Rigidbody2D rb2D;
    protected PaddleBehaviour paddleBehaviour;
    public enum ObjectState {FREE, HOLDED, THROWED, DESTROYED, MANAGE };
    public ObjectState CurrentState { get { return m_objectState; } }
    public ObjectState LastState { get { return m_lastState; } }
    public event Action <ObjectState, ObjectState> ChangeStateAction;

    public ObjectState m_objectState, m_lastState;
    public InteractibleBehaviour interactPoint;

    protected Controller controller,lastController;
    protected Vector2 velocity = Vector2.zero;

    [Header("Respawn")]
    [Range(0, 10)]
    public float respawnDelay;
    [HideInInspector]
    public Vector3 respawnPoint;
    protected bool inShip = true;


    [Header("Throw")]
    public float throwDuration = 1;
    public float throwSpeed = 3;
    public AnimationCurve throwSpeedModifier;
    protected Vector2 throwDir;
    protected float throwReadTime = 0;
    

    [Header("Holded")]
    public Vector3 holdOffset;

    [Header("Collision")]
    [Range(0,5)]
    public float collsionRange;
    public CollisionDetector collsionDetector;
    public float throwForce;
    //public float placeHolderThrowParam
    private AudioManager audioManager;

    private void OnEnable()
    {
        collsionDetector.OnCollisionPlayer += CollisionAction;
        interactPoint.InteractHappens += SetUpControl;
        ShipEvent.OnExitObj += OutShip;
        ShipEvent.OnEnterObj += InShip;
    }
    private void OnDisable()
    {
        collsionDetector.OnCollisionPlayer -= CollisionAction;
        interactPoint.InteractHappens -= SetUpControl;
        ShipEvent.OnExitObj -= OutShip;
        ShipEvent.OnEnterObj -= InShip;
    }
    private void Start()
    {
        audioManager = GameObject.Find("SoundManager").GetComponent<AudioManager>();
        respawnPoint = transform.position;
        paddleBehaviour = gameObject.GetComponent<PaddleBehaviour>();

        collsionDetector.cC2D.radius = collsionRange;
    }

    public void Update()
    {
        collsionDetector.gameObject.SetActive(m_objectState == ObjectState.THROWED);
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, shootAngle);
        switch (CurrentState)
        {
            case ObjectState.FREE:
                {
                    rb2D.bodyType = RigidbodyType2D.Kinematic;
                    break;
                }

            case ObjectState.HOLDED:
                {
                    HoldPos(controller, holdOffset);
                    break;
                }
            case ObjectState.THROWED:
                {
                    ThrowState();
                    break;
                }
        }
    }
    private void FixedUpdate()
    {
        rb2D.velocity = velocity;
        
    }

    #region Method
    void ChangeState(ObjectState newState)
    {
        Debug.Log(CurrentState + " To " + newState);
        // Exit Manage state
        if (newState != ObjectState.MANAGE && CurrentState == ObjectState.MANAGE)
            gameObject.SetActive(true);

            // Enable interaction
            if (newState != ObjectState.FREE)
            interactPoint.gameObject.SetActive(false);
        else
            interactPoint.gameObject.SetActive(true);

        // Enable action
        if(newState == ObjectState.HOLDED && CurrentState != ObjectState.HOLDED)
        {
            //Debug.Log("Enter Hold State");
            if (controller)
                controller.holdObj = true;
            InputHandler.OnAttack += AttackAction;
            InputHandler.OnInteract += InteractAction;
        }
        if (CurrentState == ObjectState.HOLDED && newState != ObjectState.HOLDED)
        {
            Debug.Log("Exit Hold State");
            
            if(controller && !(newState == ObjectState.THROWED && paddleBehaviour != null))
            {
                controller.holdObj = false;
                lastController = controller;
                controller = null;
            }
                
            InputHandler.OnAttack -= AttackAction;
            InputHandler.OnInteract -= InteractAction;

            
        }


        ChangeStateModifier(newState);
        //Debug.Log("State switch " + m_objectState + " to " + newState);
        m_lastState = m_objectState;
        m_objectState = newState;
        ChangeStateAction?.Invoke(LastState, CurrentState);
    }

    public abstract void ChangeStateModifier(ObjectState newState);
    public void HoldPos(Controller refController, Vector3 offset)
    {
        if (refController)
        {
            velocity = Vector2.zero;
            Vector3 pos = refController.transform.position;

            float holdRange = Vector2.Distance(Vector2.zero, offset);
            Vector2 lastDir = refController.pc.lastNonNullDirection.normalized;
            float holdAngle = Mathf.Atan2(lastDir.y, lastDir.x) + Mathf.Atan2(offset.y, offset.x);

            Vector2 hold2DOffset = new Vector2(Mathf.Cos(holdAngle), Mathf.Sin(holdAngle));

            transform.position = pos + (Vector3)hold2DOffset * holdRange + new Vector3(0, 0, offset.z);
            transform.localRotation = Quaternion.Euler(0, Mathf.Atan2(-lastDir.y, lastDir.x) * Mathf.Rad2Deg, 0);
            Debug.DrawRay(controller.transform.position, lastDir * 1.5f );

        }

    }

    public abstract void CollisionAction(GameObject colObject);

    public abstract void ThrowState();
  

    #region Action Property

    public void GetCaught()
    {
        if (controller != null)
        {
            velocity = Vector2.zero;
            GameObject.Find("SoundManager").GetComponent<AudioManager>().sounds[16].Play();
            ChangeState(ObjectState.HOLDED);
            HoldPos(controller, holdOffset);
        }

    }

    public void Throw( Vector2 dir, float power)
    {
        controller?.ChangeAnimationState(controller.animationState[1]);
        ChangeState(ObjectState.THROWED);

        audioManager.sounds[10].Play();
        throwDir = dir;
        throwReadTime = Time.time;
        rb2D.velocity = power * throwSpeedModifier.Evaluate((Time.time - throwReadTime) / throwDuration) * throwDir;
        Debug.Log("throw" + throwDuration);
        StartCoroutine(ThrowDuration(throwDuration));
    }
    IEnumerator ThrowDuration(float delay)
    {
        yield return new WaitForSeconds(delay);

        EndThrow();
    }

    public abstract void EndThrow();

    public void FallInGround()
    {
        ChangeState(ObjectState.FREE);
        //Debug.Log("Fall in ground");
        rb2D.velocity = velocity = Vector2.zero;
    }
    public void PutDown()
    {
        ChangeState(ObjectState.FREE);
         velocity = Vector2.zero;
    }

    public void Plouf()
    {
        rb2D.velocity = velocity = Vector2.zero;
        GetDestroy();
    }

    public void GetDestroy()
    {
        velocity = Vector2.zero;
        transform.localRotation = Quaternion.identity;
        if(controller)
            controller.holdObj = false;

        ChangeState(ObjectState.DESTROYED);
        PoolManager.instance.PerformPoolActive(respawnDelay, Respawn);
        gameObject.SetActive(false);
    }

    public void Respawn( )
    {
        Debug.Log("Respawn");
        gameObject.SetActive(true);
        transform.position = respawnPoint;
        velocity = Vector2.zero;
        ChangeState(ObjectState.FREE);
    }
    public void GetManage()
    {
        ChangeState(ObjectState.MANAGE);
        gameObject.SetActive(false);
    }

    public void OutShip(GameObject obj)
    {
        if (obj = gameObject)
            inShip = false;
    }

    public void InShip(GameObject obj)
    {
        if (obj = gameObject)
            inShip = true;
    }
    #endregion

    #region Input Action
    void SetUpControl(Controller controller)
    {
        if (!controller.holdObj)
        {
            this.controller = controller;
            lastController = controller;
            GetCaught();
        }
        
    }

    void AttackAction(Controller controller)
    {
        if(this.controller != null && controller != null)
        {
            if(this.controller == controller)
            {
                Throw(controller.pc.lastNonNullDirection, throwSpeed);
            }
        }
    }

    void InteractAction(Controller controller)
    {
        if (CurrentState == ObjectState.HOLDED)
        {
            if (inShip)
                PutDown();
            else
                Plouf();
        }
            
    }
    #endregion
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (CurrentState == ObjectState.FREE)
        Gizmos.DrawSphere(transform.position + holdOffset, 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(collsionDetector.transform.position, collsionRange);

    }
}
