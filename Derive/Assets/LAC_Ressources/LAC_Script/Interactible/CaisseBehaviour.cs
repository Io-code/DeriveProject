using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaisseBehaviour : ThrowBehaviour
{
    bool triggerCollisionAction = true;

    private void Awake()
    {
        ChangeStateAction += ResetTriggerCollision;
    }
    #region State

    void ResetTriggerCollision(ObjectState lastState, ObjectState currentState)
    {
        if (currentState == ObjectState.FREE)
        {
            triggerCollisionAction = true;
        }
    }

    public override void ThrowState()
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
    }
    #endregion
    #region Method

    public override void CollisionAction(GameObject colObject)
    {
        if (triggerCollisionAction)
        {
            triggerCollisionAction = false;
            colObject.GetComponent<Controller>().Push(transform, 10);

            FallInGround();
            Debug.Log("Fall");
        }

        //throw new System.NotImplementedException();
    }
    #endregion

    [ContextMenu("Throw")]

    void debugThrow()
    {
        Throw(Vector2.right, 10);
    }
}
