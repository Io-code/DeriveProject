using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlePoint : MonoBehaviour
{
    public InteractibleBehaviour interactPoint;
    public Controller underControl;
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
            int playerindex = CtrlToIndex(underControl, UIManager.instance.playerData);
            if (playerindex >= 0)
                UIManager.instance.playerData[playerindex].distToObjectif -= Time.deltaTime * 0.1f;
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
    }

    public int CtrlToIndex(Controller ctrl, UIPlayerData[] data)
    {
        int returnIndex = -1;
        for(int i = 0; i < data.Length; i++)
        {
            if (ctrl == data[i].refPlayer)
                returnIndex = i;
        }
        Debug.Log("CtrlToIndex : " + returnIndex);


        return returnIndex;
    }
}
