using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaderManager : MonoBehaviour
{
    public List<Vector3> waypointsPosition = new List<Vector3>();
    public List<Vector3> teleportationPosition = new List<Vector3>();

    public int index;
    public Vector3 LaderPosition(Vector3 position)
    {
        float distance = Mathf.Infinity;
        index = 0;

        for (int i = 0; i < waypointsPosition.Count; i++)
        {
            if (Vector3.Distance(position, waypointsPosition[i]) < distance)
            {
                distance = Vector3.Distance(position, waypointsPosition[i]);
                index = i;
            } 
        }
        Debug.Log(index);
        return waypointsPosition[index];
    }

    public Vector3 EndLaderPosition()
    {
        return teleportationPosition[index];
    }
}
