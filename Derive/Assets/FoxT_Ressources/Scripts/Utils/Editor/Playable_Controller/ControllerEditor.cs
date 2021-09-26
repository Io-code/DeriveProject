using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Controller))]
public class ControllerEditor : Editor
{
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
		GUILayout.Label("");
		GUILayout.Label("");
		sbyte debugDelay = 0;
		if (ctrl.debug)
		{
			debugDelay = 80;
		}
		if (GUI.Button(new Rect(5, 200 + debugDelay, Screen.width / 3 - 10, 25), "Simplified"))
		{
			ctrl.currentPushMethod = 0;
		}
		else if (GUI.Button(new Rect(5 + (Screen.width / 3), 200 + debugDelay, Screen.width / 3 - 10, 25), "Advanced"))
		{
			ctrl.currentPushMethod = 1;
		}
		else if (GUI.Button(new Rect(5 + (Screen.width / 3) * 2, 200 + debugDelay, Screen.width / 3 - 10, 25), "Curve"))
		{
			ctrl.currentPushMethod = 2;
		}
		GUILayout.Label("");
		ctrl.forceMultiplicator = EditorGUILayout.FloatField("Force Multiplicator", ctrl.forceMultiplicator);
		GUILayout.Label("");
		switch (ctrl.currentPushMethod)
		{
			case 1:
				ctrl.curveUsed = false;
				ctrl.howManyPart = EditorGUILayout.IntField("Part", ctrl.howManyPart);
				if (ctrl.howManyPart < 0)
				{
					ctrl.howManyPart = 0;
				}
				if (ctrl.lastPartNumber != ctrl.howManyPart)
				{
					ctrl.pushDelay = new float[ctrl.howManyPart];
					ctrl.pushDecelerationStep = new float[ctrl.howManyPart];
				}
				else ctrl.lastPartNumber = ctrl.howManyPart;
				GUILayout.Label("");
				for (int i = 0; i < ctrl.pushDelay.Length; i++)
				{
					GUILayout.Label("Part " + (i + 1));
					ctrl.pushDelay[i] = EditorGUILayout.FloatField("Delay", ctrl.pushDelay[i]);
					ctrl.pushDecelerationStep[i] = EditorGUILayout.FloatField("Deceleration Step", ctrl.pushDecelerationStep[i]);
					GUILayout.Label("");
				}
				break;
			case 2:
				ctrl.curveUsed = true;
				ctrl.pushCurve = EditorGUILayout.CurveField("Curve", ctrl.pushCurve);
				break;
			default:
				ctrl.curveUsed = false;
				if (ctrl.lastPushMethod != ctrl.currentPushMethod)
				{
					ctrl.pushDelay = new float[2];
					ctrl.pushDecelerationStep = new float[2];
				}
				else ctrl.lastPushMethod = ctrl.currentPushMethod;
				GUILayout.Label("Part 1");
				ctrl.pushDelay[0] = EditorGUILayout.FloatField("Delay", ctrl.pushDelay[0]);
				ctrl.pushDecelerationStep[0] = EditorGUILayout.FloatField("Deceleration Step", ctrl.pushDecelerationStep[0]);
				GUILayout.Label("");
				GUILayout.Label("Part 2");
				ctrl.pushDelay[1] = EditorGUILayout.FloatField("Delay", ctrl.pushDelay[1]);
				ctrl.pushDecelerationStep[1] = EditorGUILayout.FloatField("Deceleration Step", ctrl.pushDecelerationStep[1]);
				break;
		}
	}
}
