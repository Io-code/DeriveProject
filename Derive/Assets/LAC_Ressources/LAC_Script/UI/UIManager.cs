using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public UIPlayerData[] playerData;
    public Controller[] playerController;
    private void Awake()
    {
        UpdateSingleton();
        AssignControllerToData();
        DontDestroyOnLoad(gameObject);
    }
    
    void AssignControllerToData()
    {
        for(int i = 0; i < playerData.Length; i++)
        {
            if (i < playerController.Length)
                playerData[i].refPlayer = playerController[i];
        }
    }


    #region debug
    [ContextMenu("Update Singleton")]
    public void UpdateSingleton()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

    }
    #endregion
}
