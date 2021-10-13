using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullManager : MonoBehaviour
{
    PullManager instance;
    private void Awake()
    {
        UpdateSingleton();
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
