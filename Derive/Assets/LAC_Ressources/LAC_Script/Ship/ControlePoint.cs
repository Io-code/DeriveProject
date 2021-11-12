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
    List<GameObject> controllerObj = new List<GameObject>();

    [HideInInspector]
    public int playerIndex;
    public float scoreIncreaseSpeed = 1;
    bool startTurn = false;
    bool triggerTurn = false;
    public void OnEnable()
    {
        interactPoint.InteractHappens += SecondLead;

        colDetect.OnCollisionPlayer += AddController;
        colDetect.OnCollisionExitPlayer += RemoveController;
    }

    public void OnDisable()
    {
        interactPoint.InteractHappens -= SecondLead;

        colDetect.OnCollisionPlayer -= AddController;
        colDetect.OnCollisionExitPlayer -= RemoveController;
    }

    private void Update()
    {
        if(underControl != null)
        {
            if (encoder.gouvTurned)
            {
                startTurn = true;
                Debug.Log("Start turn Bar");
            }
                

            if (playerIndex >= 0 && startTurn)
            {
                PlayerDataUtils.UpdateScore(UIManager.instance.playerData[playerIndex], scoreIncreaseSpeed);
            }
                
        }
    }
    public void SecondLead(Controller ctrl)
    {
        startTurn = true;
    }
    public void SetUpShipLead( List<GameObject> obj)
    {
        if (obj.Count == 0)
        {
            Debug.Log("Reset turn Bar");
            startTurn = false;
            //underControl = null;
        }
        else if (underControl != obj[0].GetComponent<Controller>())
            triggerTurn = true;

        if(obj.Count == 0)
            underControl = obj[0].GetComponent<Controller>();

        playerIndex = CtrlToIndex(underControl, UIManager.instance.playerData);
    }

    public void AddController(GameObject obj)
    {
        if (!controllerObj.Contains(obj))
            controllerObj.Add(obj);

        SetUpShipLead(controllerObj);
        Debug.Log("Add Controller");
    }
     public void RemoveController(GameObject obj)
    {
        if (controllerObj.Contains(obj))
            controllerObj.Remove(obj);

        SetUpShipLead(controllerObj);
        Debug.Log("Remove Controller");
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
