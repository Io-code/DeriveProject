using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/UIPlayerData", order = 1)]
public class UIPlayerData : ScriptableObject
{
    [Range(0, 100)]
    public float distToObjectif;
    public Controller refPlayer;

    public bool[] winRound = new bool[3];
    public float lastInputTime;
}
