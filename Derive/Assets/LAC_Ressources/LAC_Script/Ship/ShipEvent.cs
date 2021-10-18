using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ShipEvent 
{
    public static Action<GameObject> OnExitObj, OnEnterObj, OnExitPlayer;
    public static void PerformExitObj(GameObject exitObj)
    {
        //Debug.Log("exit obj " + exitObj.name);
        OnExitObj?.Invoke(exitObj);
    }
    public static void PerformEnterObj(GameObject enterObj)
    {
        OnEnterObj?.Invoke(enterObj);
    }

    public static void PerformExitPlayer(GameObject player)
    {
        //Debug.Log("exit player " + player.name);
        OnExitPlayer?.Invoke(player);
    }
}
