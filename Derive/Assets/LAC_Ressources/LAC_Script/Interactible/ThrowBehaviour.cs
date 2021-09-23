using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThrowBehaviour : MonoBehaviour
{
    public enum ObjectState {FREE, HOLDED, THROWED, DESTROYED };
    public ObjectState CurrentState { get { return m_objectState; } }
    public ObjectState LastState { get { return m_lastState; } }

    ObjectState m_objectState, m_lastState;

    [Header("Respawn")]
    public Transform respawnPoint;
    [Range(0,10)]
    public float respawnDelay;

    [Header("Throw")]
    public string placeHolder;

    [Header("Holded")]
    public float holdRange;
    public Vector3 holdOffset;
    [HideInInspector]
    public Transform holdRef;

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
        switch (newState)
        {
            case ObjectState.FREE:
                {
                    break;
                }

            case ObjectState.HOLDED:
                {
                    break;
                }
            case ObjectState.THROWED:
                {

                    break;
                }
            case ObjectState.DESTROYED:
                {
                    break;
                }
        }

        m_lastState = m_objectState;
        m_objectState = newState;
    }

    #region Action Property
    public void Throw( Vector2 dir)
    {
        ChangeState(ObjectState.THROWED);
    }
    public void PutDown()
    {
        ChangeState(ObjectState.FREE);
    }

    public void GetCaught()
    {
        ChangeState(ObjectState.HOLDED);
        transform.position = holdRef.position;
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


}
