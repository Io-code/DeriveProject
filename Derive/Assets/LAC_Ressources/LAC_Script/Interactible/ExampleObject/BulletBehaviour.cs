using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour :ThrowBehaviour
{
    public enum BulletState { UNLOAD,LOAD, THROWED, EXPLODED };
    public BulletState bulletState, lasteBulletState;
    bool triggerCollisionAction = true;

    private void Awake()
    {
        ChangeStateAction += ResetTriggerCollision;
    }
    #region State
    void ChangeBulletState( BulletState state)
    {
        lasteBulletState = bulletState;
        bulletState = state;
    }
    void ResetTriggerCollision(ObjectState lastState, ObjectState currentState)
    {
        if(currentState == ObjectState.FREE)
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

    public override void EndThrow()
    {
        if (CurrentState == ObjectState.THROWED)
        {
            if (inShip)
                FallInGround();
            else
                Plouf();
        }
    }

    #endregion
    #region Method
    public void Load()
    {
        GetManage();
        ChangeBulletState(BulletState.LOAD);
    }

    public void Shoot( Vector3 shootPos, Vector2 dir, float power)
    {
        transform.position = shootPos;
        ChangeBulletState(BulletState.THROWED);
        Throw(dir, power);
    }

    public override void CollisionAction(GameObject colObject)
    {
        if (triggerCollisionAction && colObject != controller?.gameObject)
        {
            triggerCollisionAction = false;
            ChangeBulletState(BulletState.EXPLODED);

            colObject.GetComponent<Controller>().Push(transform, throwForce);

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
