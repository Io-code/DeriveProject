using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LaderManager))]
public class LaderManager_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        LaderManager lm = (LaderManager)target;
        //base.OnInspectorGUI();
        if (GUILayout.Button("Add Waypoint"))
        {
            lm.waypointsPosition.Add(Vector3.zero);
        }
        else if (GUILayout.Button("Remove Last") && lm.waypointsPosition.Count != 0)
        {
            lm.waypointsPosition.Remove(lm.waypointsPosition[lm.waypointsPosition.Count - 1]);
        }        

        GUILayout.Label("");
        GUILayout.Label("Waypoint : " + lm.waypointsPosition.Count);
        GUILayout.Label("");
        for (int i = 0; i < lm.waypointsPosition.Count; i++)
        {
            lm.waypointsPosition[i] = EditorGUILayout.Vector3Field("Waypoint " + i.ToString(), lm.waypointsPosition[i]);
        }
        SceneView.RepaintAll();
    }

    private protected virtual void OnSceneGUI()
    {
        LaderManager lm = (LaderManager)target;
        Handles.color = Color.green;
        foreach (Vector3 v in lm.waypointsPosition)
        {
            Handles.SphereHandleCap(0, v, lm.transform.rotation, 1f, EventType.Repaint);
        }
    }
}
