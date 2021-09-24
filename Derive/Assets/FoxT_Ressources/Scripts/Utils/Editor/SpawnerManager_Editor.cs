using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnerManager))]
public class SpawnerManager_Editor : Editor
{
    public List<GameObject> spawner = new List<GameObject>();
    public override void OnInspectorGUI()
    {
        SpawnerManager spw = (SpawnerManager)target;
        //base.OnInspectorGUI();
        GUI.color = Color.cyan;
        EditorGUILayout.LabelField("Spawner Informations");
        EditorGUILayout.LabelField("");

        GUI.color = Color.white;
        spw.uniqueSpawner = EditorGUILayout.Toggle("All Spawner Can Spawn All Item", spw.uniqueSpawner);

        if (!spw.uniqueSpawner)
        {
            
        }
    }


}

