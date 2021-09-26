using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Fox.Editor;

[CustomEditor(typeof(Controller))]
public class ControllerEditor : Editor
{
	int test;
	public override void OnInspectorGUI()
	{
		Controller ctrl = (Controller)target;
		//base.OnInspectorGUI();
		ctrl.debug = EditorGUILayout.Toggle("Enable Debug", ctrl.debug);

		//Movement
		GUI.color = Color.cyan;
		GUILayout.Label("");
		GUILayout.Label("Movement");

		GUI.color = Color.white;
		ctrl.maxSpeed = EditorGUILayout.FloatField("Max Speed", ctrl.maxSpeed);
		if (ctrl.debug)
		{
			GUI.color = Color.green;
			ctrl.minAccelerationStep = EditorGUILayout.FloatField("min Acce Slider", ctrl.minAccelerationStep);
			ctrl.maxAccelerationStep = EditorGUILayout.FloatField("max Acce Slider", ctrl.maxAccelerationStep);
			ctrl.minDecelerationStep = EditorGUILayout.FloatField("min Dece Slider", ctrl.minDecelerationStep);
			ctrl.maxDecelerationStep = EditorGUILayout.FloatField("max Dece Slider", ctrl.maxDecelerationStep);
			GUI.color = Color.white;
		}
		ctrl.accelerationStep = EditorGUILayout.Slider("Acceleration Step", ctrl.accelerationStep, ctrl.minAccelerationStep, ctrl.maxAccelerationStep);
		ctrl.decelerationStep = EditorGUILayout.Slider("Deceleration Step", ctrl.decelerationStep, ctrl.minDecelerationStep, ctrl.maxDecelerationStep);

		//Push
		GUI.color = Color.cyan;
		GUILayout.Label("");
		GUILayout.Label("Push");
		
		GUI.color = Color.white;
		GUILayout.Label("Methods");
		GUILayout.Label("");
		CurveOptionsEditor.CurveFields(ctrl.curves);
	}
}
