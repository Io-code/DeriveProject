using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBehaviour : ThrowBehaviour
{
    bool triggerCollisionAction = true;
    public bool paddleActive = true;
    public float maxUse = 3;
    float currentUse = 0;

    public Controller lastController;

    public override void CollisionAction(GameObject colObject)
    {
        if (triggerCollisionAction && colObject != controller.gameObject)
        {
            triggerCollisionAction = false;
            colObject.GetComponent<Controller>().Push(transform, 10);
            currentUse++;
            if (currentUse > maxUse)
                GetDestroy();
        }
    }

    public override void ThrowState()
    {
        if (controller)
        {
            lastController = controller;

            velocity = Vector2.zero;
            Vector3 pos = controller.transform.position;

            float holdRange = Vector2.Distance(Vector2.zero, holdOffset);
            Vector2 lastDir = controller.pc.lastNonNullDirection.normalized;
            float holdAngle = Mathf.Atan2(lastDir.y, lastDir.x) + Mathf.Atan2(holdOffset.y, holdOffset.x);

            Vector2 hold2DOffset = new Vector2(Mathf.Cos(holdAngle), Mathf.Sin(holdAngle));

            transform.position = pos + (Vector3)hold2DOffset * holdRange + new Vector3(0, 0, holdOffset.z);
            //transform.localRotation = Quaternion.Euler(0, Mathf.Atan2(-lastDir.y, lastDir.x) * Mathf.Rad2Deg, 0);
            Debug.DrawRay(controller.transform.position, lastDir * 1.5f);

        }

        collsionDetector.gameObject.SetActive(paddleActive);
        transform.Rotate(0, -60 * Time.deltaTime, 0);
        //transform.eulerAngles= new Vector3 (transform.eulerAngles.x +60 * Time.deltaTime, transform.eulerAngles.y , transform.eulerAngles.z);
        //Debug.Log("Pos " + new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z));
        //HoldPos(controller, holdOffset);
    }

    public override void EndThrow()
    {
        if (CurrentState == ObjectState.THROWED)
        {
            Debug.Log("End ThrowState");
            if (!inShip)
                Plouf();
            else
            {
                controller = lastController;
                GetCaught();
            }  
        }
    }

    public override void ChangeStateModifier(ObjectState newState)
    {

    }

    void UpdatePaddleController(ObjectState currentState, ObjectState newState)
    {
        if (newState == ObjectState.THROWED)
            lastController = controller;
    }
}
