using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class ThrowBehaviour : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public enum ObjectState {FREE, HOLDED, THROWED, DESTROYED };
    public ObjectState CurrentState { get { return m_objectState; } }
    public ObjectState LastState { get { return m_lastState; } }

    public ObjectState m_objectState, m_lastState;
    public InteractibleBehaviour interactPoint;
    Controller controller;
    Vector2 velocity = Vector2.zero;

    [Header("Respawn")]
    public Transform respawnPoint;
    [Range(0,10)]
    public float respawnDelay;

    [Header("Throw")]
    public float throwDuration;
    public float throwSpeed;
    public AnimationCurve throwSpeedModifier;
    Vector2 throwDir;
    float throwReadTime = 0;
    

    [Header("Holded")]
    public Vector3 holdOffset;

    [Header("Collision")]
    [Range(0,5)]
    public float collsionRange;
    public List<string> collsionTag = new List<string>();
    //public float placeHolderThrowParam


    private void OnEnable()
    {
        interactPoint.InteractHappens += SetUpControl;
    }
    private void OnDisable()
    {
        interactPoint.InteractHappens -= SetUpControl;
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
                    if (rb2D.velocity.magnitude < 0.1f && CurrentState == ObjectState.THROWED)
                        FallInGround();

                    // detect 
                    
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

        // Enable interaction
        if (newState != ObjectState.FREE)
            interactPoint.gameObject.SetActive(false);
        else
            interactPoint.gameObject.SetActive(true);

        // Enable action
        if(newState == ObjectState.HOLDED)
        {
            InputHandler.Instance.OnAttack += AttackAction;
            InputHandler.Instance.OnInteract += InteractAction;
        }
        if (LastState == ObjectState.HOLDED && LastState != newState)
        {
            InputHandler.Instance.OnAttack -= AttackAction;
            InputHandler.Instance.OnInteract -= InteractAction;
        }

        m_lastState = m_objectState;
        m_objectState = newState;
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
            transform.eulerAngles = new Vector3(transform.eulerAngles.x + Mathf.Cos(lastDir.x) * Mathf.Rad2Deg, transform.eulerAngles.y + Mathf.Sin(lastDir.y) * Mathf.Rad2Deg, transform.eulerAngles.z);
        }

    }

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
    public void Throw( Vector2 dir)
    {
        ChangeState(ObjectState.THROWED);
        throwDir = dir;
        throwReadTime = Time.time;
        rb2D.velocity = throwSpeed * throwSpeedModifier.Evaluate((Time.time - throwReadTime) / throwDuration) * throwDir;
        StartCoroutine(EndThrow(throwDuration));
    }

    IEnumerator EndThrow(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (CurrentState == ObjectState.THROWED)
            FallInGround();


    }
    public void FallInGround()
    {
        ChangeState(ObjectState.FREE);
        velocity = Vector2.zero;
    }

    public void PutDown()
    {
        ChangeState(ObjectState.FREE);
        velocity = Vector2.zero;
    }



    public void Respawn()
    {
        transform.position = respawnPoint.position;
        ChangeState(ObjectState.FREE);
    }
    public void GetDestroy()
    {
        
        ChangeState(ObjectState.DESTROYED);
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
                Throw(controller.pc.lastNonNullDirection);
            }
        }
    }

    void InteractAction(Controller controller)
    {
        if (CurrentState == ObjectState.HOLDED)
            PutDown();
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
