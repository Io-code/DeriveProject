using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(PositionReset());
    }
    private void Update()
    {
        
            transform.localPosition = Vector2.zero;
    }

    private IEnumerator PositionReset()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
        }
    }
}
