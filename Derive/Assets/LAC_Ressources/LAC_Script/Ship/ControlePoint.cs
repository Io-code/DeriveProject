using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlePoint : MonoBehaviour
{
    public InteractibleBehaviour interactPoint;
    [HideInInspector]
    public Controller underControl;
    [HideInInspector]
    public int playerIndex;
    public float scoreIncreaseSpeed = 1;
    public void OnEnable()
    {
        interactPoint.InteractHappens += SetUpShipLead;
    }

    public void OnDisable()
    {
        interactPoint.InteractHappens -= SetUpShipLead;
    }

    private void Update()
    {
        if(underControl != null)
        {
          
            if (playerIndex >= 0)
                PlayerDataUtils.UpdateScore(UIManager.instance.playerData[playerIndex], scoreIncreaseSpeed);
        }
    }
    public void SetUpShipLead( Controller player)
    {
        if (interactPoint.players.Count == 1)
        {
            underControl = player;
            
        }
        else
            underControl = null;
        playerIndex = CtrlToIndex(underControl, UIManager.instance.playerData);
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
