using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] public Level level;
}

[Serializable]
public class Level
{
    public Unit[] units;
    public Props[] props;
}
[Serializable]
public class Unit
{
    public int life;
}
[Serializable]
public class Props
{
    public int durability;
}
