using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathExtension;

public class ExtensionTest : MonoBehaviour
{
    private Vector3 test = new Vector3(1, 2, 3);
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Mathe.V3toV2(test));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
