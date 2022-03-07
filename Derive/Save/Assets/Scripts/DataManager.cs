using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] DataSerializer dataSerializer;

    public void Save()
    {
        dataSerializer.SaveData(levelManager.level);
    }

    public void Load()
    {
        levelManager.level = dataSerializer.LoadData<Level>();
    }
}
