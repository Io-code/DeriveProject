using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaderManager : MonoBehaviour
{
    public List<Vector3> waypointsPosition = new List<Vector3>();

    public Vector3 LaderPosition(Vector3 position)
    {
        float distance = Mathf.Infinity;
        int index = 0;

        for (int i = 0; i < waypointsPosition.Count; i++)
        {
            if (Vector3.Distance(position, waypointsPosition[i]) < distance) index = i;
        }
        return waypointsPosition[index];
    }
}
