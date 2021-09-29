using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThrowBehaviour : MonoBehaviour
{
    public enum ObjectState {FREE, HOLDED, THROWED, DESTROYED };
    public ObjectState CurrentState { get { return m_objectState; } }
    public ObjectState LastState { get { return m_lastState; } }

    public ObjectState m_objectState, m_lastState;
    public InteractibleBehaviour interactPoint;
    Controller controller;

    [Header("Respawn")]
    public Transform respawnPoint;
    [Range(0,10)]
    public float respawnDelay;

    [Header("Throw")]
    public string placeHolderParam;

    [Header("Holded")]
    public Vector3 holdOffset;

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

                    break;
                }

            case ObjectState.HOLDED:
                {
                    HoldPos(controller, holdOffset);
                    break;
                }
            case ObjectState.THROWED:
                {

                    break;
                }

        }
    }

    void ChangeState(ObjectState newState)
    {

        // Enable interaction
        if (LastState == ObjectState.FREE && LastState != newState)
            interactPoint.gameObject.SetActive(false);
        if(newState == ObjectState.FREE)
            interactPoint.gameObject.SetActive(false);

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
            Vector3 pos = refController.transform.position;

            float holdRange = Vector2.Distance(Vector2.zero, offset);
            Vector2 lastDir = refController.pc.lastNonNullDirection.normalized;
            Vector2 hold2DOffset =  (lastDir + (Vector2)offset.normalized) * new Vector2(Mathf.Sign(lastDir.x),1);

            transform.position = pos + (Vector3)hold2DOffset * holdRange + new Vector3(0, 0, offset.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x + Mathf.Cos(lastDir.x) * Mathf.Rad2Deg, transform.eulerAngles.y + Mathf.Sin(lastDir.y) * Mathf.Rad2Deg, transform.eulerAngles.z);
        }

    }

    #region Action Property

    public void GetCaught()
    {
        if (controller != null)
        {
            ChangeState(ObjectState.HOLDED);
            HoldPos(controller, holdOffset);
        }

    }
    public void Throw( Vector2 dir)
    {
        ChangeState(ObjectState.THROWED);
    }
    public void PutDown()
    {
        ChangeState(ObjectState.FREE);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + holdOffset, 0.2f);
    }
}
