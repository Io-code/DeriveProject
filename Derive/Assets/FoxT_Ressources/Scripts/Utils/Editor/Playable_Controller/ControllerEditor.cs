using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Controller))]
public class ControllerEditor : Editor
{
	private bool debug;
	//Debug
	//Movement elements
	private float minAccelerationStep = 1, maxAccelerationStep = 10;
	private float minDecelerationStep = 1, maxDecelerationStep = 10;
	public override void OnInspectorGUI()
	{
		Controller ctrl = (Controller)target;
		//base.OnInspectorGUI();
		debug = EditorGUILayout.Toggle("Enable Debug", debug);

		GUI.color = Color.cyan;
		GUILayout.Label("");
		GUILayout.Label("Movement");
		GUILayout.Label("");

		GUI.color = Color.white;
		ctrl.maxSpeed = EditorGUILayout.FloatField("Max Speed", ctrl.maxSpeed);
		if (debug)
		{
			GUI.color = Color.green;
			minAccelerationStep = EditorGUILayout.FloatField("min Acce Slider", minAccelerationStep);
			maxAccelerationStep = EditorGUILayout.FloatField("max Acce Slider", maxAccelerationStep);
			minDecelerationStep = EditorGUILayout.FloatField("min Dece Slider", minDecelerationStep);
			maxDecelerationStep = EditorGUILayout.FloatField("max Dece Slider", maxDecelerationStep);
			GUI.color = Color.white;
		}
		ctrl.accelerationStep = EditorGUILayout.Slider("Acceleration Step", ctrl.accelerationStep, minAccelerationStep, maxAccelerationStep);
		ctrl.decelerationStep = EditorGUILayout.Slider("Deceleration Step", ctrl.decelerationStep, minDecelerationStep, maxDecelerationStep);
	}
}
