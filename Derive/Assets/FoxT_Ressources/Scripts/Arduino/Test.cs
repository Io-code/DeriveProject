using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    ReadEncoder re;
    void Start()
    {
        re = GameObject.Find("Uduino").GetComponent<ReadEncoder>();
        StartCoroutine(TestCoroutine());
    }

    private IEnumerator TestCoroutine()
    {
        while (true)
        {
            StartCoroutine(re.StartRead(5, true));
            float timeElapsed = 0;
            while (timeElapsed <= 6)
            { 
                yield return new WaitForEndOfFrame();
                if (re.fpTour == 1) Debug.Log("YAAAAAAAAAAAAAA");
                else if (re.fpTour == -1) Debug.Log("NNNNNNNNNNNNNNNONNNNNN");
                timeElapsed += Time.deltaTime;
            }
        }
    }
}
