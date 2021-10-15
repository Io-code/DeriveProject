using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDataUtils 
{
    public static void ResetPlayerData(UIPlayerData playerData)
    {
        playerData.distToObjectif = 100;
    }

    public static void UpdateScore(UIPlayerData playerData, float decreaseSpeed)
    {
        playerData.distToObjectif -= decreaseSpeed * Time.deltaTime;
    }
}
