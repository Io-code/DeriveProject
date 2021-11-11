using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlePoint : MonoBehaviour
{
    public InteractibleBehaviour interactPoint;
    public CollisionDetector colDetect;
    public ReadEncoder encoder;

    [HideInInspector]
    public Controller underControl;
    List<GameObject> controllerObj;

    [HideInInspector]
    public int playerIndex;
    public float scoreIncreaseSpeed = 1;
    public void OnEnable()
    {
        //interactPoint.InteractHappens += SetUpShipLead;

        colDetect.OnCollisionPlayer += AddController;
        colDetect.OnCollisionExitPlayer += RemoveController;
    }

    public void OnDisable()
    {
        //interactPoint.InteractHappens -= SetUpShipLead;

        colDetect.OnCollisionPlayer -= AddController;
        colDetect.OnCollisionExitPlayer -= RemoveController;
    }

    private void Update()
    {
        if(underControl != null)
        {
          
            if (playerIndex >= 0)
            {
                PlayerDataUtils.UpdateScore(UIManager.instance.playerData[playerIndex], scoreIncreaseSpeed);
            }
                
        }
    }
    public void SetUpShipLead( List<GameObject> obj)
    {
        if (obj.Count == 0)
            underControl = null;
        else
            underControl = obj[0].GetComponent<Controller>();

        playerIndex = CtrlToIndex(underControl, UIManager.instance.playerData);
    }

    public void AddController(GameObject obj)
    {
        if (!controllerObj.Contains(obj))
            controllerObj.Add(obj);

        SetUpShipLead(controllerObj);
    }
     public void RemoveController(GameObject obj)
    {
        if (controllerObj.Contains(obj))
            controllerObj.Remove(obj);

        SetUpShipLead(controllerObj);
    }
    public int CtrlToIndex(Controller ctrl, UIPlayerData[] data)
    {
        int returnIndex = -1;
        for(int i = 0; i < data.Length; i++)
        {
            if (ctrl == data[i].refPlayer)
                returnIndex = i;
        }
        //Debug.Log("CtrlToIndex : " + returnIndex);


        return returnIndex;
    }
}
