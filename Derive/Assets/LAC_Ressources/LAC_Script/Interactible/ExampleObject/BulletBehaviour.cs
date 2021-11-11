using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour :ThrowBehaviour
{
    public enum BulletState { UNLOAD,LOAD, THROWED, EXPLODED };
    public BulletState bulletState, lasteBulletState;
    public float explosionRadius = 2;
    bool triggerCollisionAction = true;

    public GameObject explodeVFX;

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
        collsionDetector.cC2D.radius = (bulletState == BulletState.LOAD) ? collsionRange : explosionRadius;
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        velocity = throwSpeed * throwSpeedModifier.Evaluate((Time.time - throwReadTime) / throwDuration) * throwDir;
        if (rb2D.velocity.magnitude < 0.1f && CurrentState == ObjectState.THROWED && ((Time.time - throwReadTime) / throwDuration) > 0.2f)
        {
            if (inShip)
            {
                if (bulletState == BulletState.THROWED)
                    Explode();
                else
                    FallInGround();
            }
                
            else
                Plouf();
        }
    }

    public override void EndThrow()
    {
        collsionDetector.cC2D.radius = collsionRange;
        if (CurrentState == ObjectState.THROWED)
        {
            if (inShip)
            {

                FallInGround();
            }
                
            else
                Plouf();
        }
    }

    public override void ChangeStateModifier(ObjectState newState)
    {
      
    }

    #endregion
    #region Method
    public void Load()
    {
        
        GetManage();
        ChangeBulletState(BulletState.LOAD);
        lastController = null;
        
        //controller = null;
    }

    public void Shoot( Vector2 dir, float power)
    {
        ChangeBulletState(BulletState.THROWED);
        Throw(dir, power);
    }

    public void Explode()
    {
        Debug.Log("BOOM");
        Instantiate(explodeVFX, transform.position, transform.rotation);
        ChangeBulletState(BulletState.EXPLODED);
        GetDestroy();
    }

    public override void CollisionAction(GameObject colObject)
    {
        if (triggerCollisionAction && colObject != lastController?.gameObject)
        {
            triggerCollisionAction = false;
            if(bulletState == BulletState.THROWED)
            {
                Explode();
                Debug.Log(bulletState);
            }
                

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
