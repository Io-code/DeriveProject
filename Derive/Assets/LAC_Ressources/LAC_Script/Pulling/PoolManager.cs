using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    public delegate void ReplaceDelegate();
    public ReplaceDelegate replaceDelegate;
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
    public void PerformPoolActive(float delay, Action method )
    {
        StartCoroutine(PoolActive(delay, method));
    }
    IEnumerator PoolActive(float delay, Action method)
    {
        yield return new WaitForSeconds(delay);
        method?.Invoke() ;
    }
}
