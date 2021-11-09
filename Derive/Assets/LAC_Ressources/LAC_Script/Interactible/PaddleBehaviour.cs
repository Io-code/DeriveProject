using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBehaviour : ThrowBehaviour
{
    bool triggerCollisionAction = true;
    public bool paddleActive = true;
    public float maxUse = 3;
    float currentUse = 0;
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
        collsionDetector.gameObject.SetActive(paddleActive);
        HoldPos(controller, holdOffset);
    }
}
