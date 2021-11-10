using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Fox.Editor;

[CustomEditor(typeof(Controller))]
public class ControllerEditor : Editor
{
	int animationIndex;
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
		ctrl.swimSpeed = Mathf.Abs(EditorGUILayout.FloatField("Swiming Speed", ctrl.swimSpeed));

		//Push
		GUI.color = Color.cyan;
		GUILayout.Label("");
		GUILayout.Label("Push");
		
		GUI.color = Color.white;
		GUILayout.Label("Methods");
		GUILayout.Label("");
		CurveOptionsEditor.CurveFields(ctrl.curves);

		GUI.color = Color.cyan;
		GUILayout.Label("");
		GUILayout.Label("Animation");
		GUI.color = Color.white;
		GUILayout.Label("");

		/*animationIndex = EditorGUILayout.IntField("How Many Animations", animationIndex);
		if (animationIndex != ctrl.animationState.Length)
		{
			string[] animationStateTemp = new string[ctrl.animationState.Length];
			for (int i = 0; i < animationStateTemp.Length; i++)
			{
				animationStateTemp[i] = ctrl.animationState[i];
			}

			int max = 0;
			if (animationIndex > ctrl.animationState.Length) max = animationStateTemp.Length;
			else if (animationIndex < ctrl.animationState.Length) max = animationIndex;
			ctrl.animationState = new string[animationIndex];

			for (int i = 0; i < max; i++)
			{
				ctrl.animationState[i] = animationStateTemp[i];
			}
		}
		for (int i = 0; i < ctrl.animationState.Length; i++)
		{
			ctrl.animationState[i] = EditorGUILayout.TextField("Animation Name " + i, ctrl.animationState[i]);
		}*/
	}
}
