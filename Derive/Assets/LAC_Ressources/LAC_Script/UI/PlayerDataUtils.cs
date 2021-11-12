using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PlayerDataUtils 
{
    public static Action<UIPlayerData> winRound;
    public static void ResetPlayerData(UIPlayerData playerData)
    {
        playerData.distToObjectif = 100;
        playerData.lastInputTime = 0;

        for (int i = 0; i < playerData.winRound.Length; i++)
            playerData.winRound[i] = false;

        
    }
   
    public static void UpdateScore(UIPlayerData playerData, float decreaseSpeed)
    {
        playerData.distToObjectif -= decreaseSpeed * Time.deltaTime;
        if (playerData.distToObjectif <= 0)
        {
            ResetScore(playerData);
            Debug.Log("Win Score" + playerData.name);
            winRound?.Invoke(playerData);
            
        }
            
    }


    public static void ResetScore(UIPlayerData playerData)
    {
        playerData.distToObjectif = 100;
    }

    public static void ResetRound(UIData uiData)
    {
        uiData.round = 0;
    }

    public static void UpdateRound(UIData uiData)
    {
        uiData.round += 1;
    }

}
