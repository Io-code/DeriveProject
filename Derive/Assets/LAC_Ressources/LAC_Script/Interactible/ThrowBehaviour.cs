using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class ThrowBehaviour : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public enum ObjectState {FREE, HOLDED, THROWED, DESTROYED, MANAGE };
    public ObjectState CurrentState { get { return m_objectState; } }
    public ObjectState LastState { get { return m_lastState; } }
    public event Action <ObjectState, ObjectState> ChangeStateAction;

    public ObjectState m_objectState, m_lastState;
    public InteractibleBehaviour interactPoint;

    Controller controller;
    Vector2 velocity = Vector2.zero;

    [Header("Respawn")]
    public Transform respawnPoint;
    bool inShip = true;
    [Range(0,10)]
    public float respawnDelay;

    [Header("Throw")]
    public float throwDuration = 1;
    public float throwSpeed = 3;
    public AnimationCurve throwSpeedModifier;
    Vector2 throwDir;
    float throwReadTime = 0;
    

    [Header("Holded")]
    public Vector3 holdOffset;

    [Header("Collision")]
    [Range(0,5)]
    public float collsionRange;
    public CollisionDetector collsionDetector;
    //public float placeHolderThrowParam

    private void OnEnable()
    {
        interactPoint.InteractHappens += SetUpControl;
        ShipEvent.OnExitObj += OutShip;
        ShipEvent.OnEnterObj += InShip;
    }
    private void OnDisable()
    {
        interactPoint.InteractHappens -= SetUpControl;
        ShipEvent.OnExitObj -= OutShip;
        ShipEvent.OnEnterObj -= InShip;
    }
    private void Start()
    {
        collsionDetector.cC2D.radius = collsionRange;
    }

    public void Update()
    {
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
                    rb2D.bodyType = RigidbodyType2D.Dynamic;
                    velocity = throwSpeed * throwSpeedModifier.Evaluate((Time.time - throwReadTime) / throwDuration) * throwDir;
                    if (rb2D.velocity.magnitude < 0.1f && CurrentState == ObjectState.THROWED && ((Time.time - throwReadTime) / throwDuration) > 0.2f)
                    {
                        if (inShip)
                            FallInGround();
                        else
                            Plouf();
                    }
                       
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
            InputHandler.OnAttack += AttackAction;
            InputHandler.OnInteract += InteractAction;
        }
        if (LastState == ObjectState.HOLDED && LastState != newState)
        {
            InputHandler.OnAttack -= AttackAction;
            InputHandler.OnInteract -= InteractAction;
        }

        
        if(newState == ObjectState.THROWED && CurrentState != ObjectState.THROWED)
        {
            collsionDetector.OnCollisionPlayer += CollisionAction;
        }
        if (newState != ObjectState.THROWED && CurrentState == ObjectState.THROWED)
        {
            //Debug.Log("unsuscribe");
            collsionDetector.OnCollisionPlayer -= CollisionAction;
        }

        //Debug.Log("State switch " + m_objectState + " to " + newState);
        m_lastState = m_objectState;
        m_objectState = newState;
        ChangeStateAction?.Invoke(LastState, CurrentState);
    }
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
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, holdAngle * Mathf.Rad2Deg);
        }

    }

    public abstract void CollisionAction(GameObject colObject);
  

    #region Action Property

    public void GetCaught()
    {
        if (controller != null)
        {
            velocity = Vector2.zero;
            ChangeState(ObjectState.HOLDED);
            HoldPos(controller, holdOffset);
        }

    }

    public void Throw( Vector2 dir, float power)
    {
        ChangeState(ObjectState.THROWED);
        throwDir = dir;
        throwReadTime = Time.time;
        rb2D.velocity = power * throwSpeedModifier.Evaluate((Time.time - throwReadTime) / throwDuration) * throwDir;
        Debug.Log("throw" + throwDuration);
        StartCoroutine(EndThrow(throwDuration));
    }
    IEnumerator EndThrow(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (CurrentState == ObjectState.THROWED)
        {
            if (inShip)
                FallInGround();
            else
                Plouf();
        }
 
    }

    public void FallInGround()
    {
        ChangeState(ObjectState.FREE);
        Debug.Log("Fall in ground");
        rb2D.velocity = velocity = Vector2.zero;
    }
    public void PutDown()
    {
        ChangeState(ObjectState.FREE);
        controller = null;
         velocity = Vector2.zero;
    }

    public void Plouf()
    {
        rb2D.velocity = velocity = Vector2.zero;
        GetDestroy();
    }

    public void GetDestroy()
    {
        ChangeState(ObjectState.DESTROYED);
        PoolManager.instance.PerformPoolActive(respawnDelay, Respawn);
        gameObject.SetActive(false);
    }

    public void Respawn( )
    {
        Debug.Log("Respawn");
        gameObject.SetActive(true);
        transform.position = respawnPoint.position;
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
        this.controller = controller;
        GetCaught();
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
        Gizmos.DrawWireSphere(transform.position, collsionRange);
    }
}
