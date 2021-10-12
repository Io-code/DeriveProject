using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Fox.Editor
{
	[System.Serializable]
	public class CurveOptions
	{
		public CurveMethods currentMethod = CurveMethods.Simplified;
		public uint curveKeys;
		public float forceMultiplicator;
		//Simplified & Advanced methods
		public float[] delay;
		public float[] decelerationStep;
		//Curve Methods
		public AnimationCurve curve;

		public bool isPlaying;
		public Vector2 velocity;
		public CurveOptions()
		{
			delay = new float[0];
			decelerationStep = new float[0];
		}

		public string GetCurrentMethods()
		{
			return currentMethod.ToString();
		}

		public IEnumerator Play(Vector2 dir, float force)
		{
			isPlaying = true;
			dir = dir.normalized;
			float timeElapsed = 0;
			float currentForce = force;
			if (currentMethod == CurveMethods.Curve)
			{
				while (true)
				{
					velocity = dir * curve.Evaluate(timeElapsed) * force * forceMultiplicator * 10;
					if (timeElapsed >= curve.keys[curve.keys.Length - 1].time) break;
					yield return new WaitForEndOfFrame();
					timeElapsed += Time.deltaTime;
				}
			}
			else
			{
				float forceSubstracted = 0;
				for (int i = 0; i < delay.Length; i++)
				{
					timeElapsed = 0;
					while (true)
					{
						velocity = dir * currentForce * forceMultiplicator * 10;
						try
						{
							currentForce = Mathf.Clamp(force - (force * decelerationStep[i] * (timeElapsed / delay[i])) - forceSubstracted, 0, force);
						}
						catch
						{
							Debug.LogError("On controller, about Push fonctions, Delay can't be equal to 0");
							break;
						}
						if (timeElapsed >= delay[i]) break;
						yield return new WaitForEndOfFrame();
						timeElapsed += Time.deltaTime;
					}
					forceSubstracted += force * decelerationStep[i];
				}
			}
			isPlaying = false;
		}  
    }

	public static class CurveOptionsEditor
	{
		public static void CurveFields(CurveOptions method)
		{
			method.currentMethod = (CurveMethods)GUILayout.Toolbar((int)method.currentMethod, new string[] { "Simplified", "Advanced", "Curve" });
			GUILayout.Label("");
			method.forceMultiplicator = EditorGUILayout.FloatField("Force Multiplicator", method.forceMultiplicator);
			switch (method.currentMethod)
			{
				default:
					method.curveKeys = 2;
					NonCurveOptions(method.curveKeys, method);
					break;
				case CurveMethods.Advanced:
					method.curveKeys = (uint)EditorGUILayout.IntField("Part", (int)method.curveKeys);
					NonCurveOptions(method.curveKeys, method);
					break;
				case CurveMethods.Curve:
					method.curve = EditorGUILayout.CurveField("Curve", method.curve);
					break;
			}
		}

		public static void NonCurveOptions(uint part, CurveOptions method)
		{
			if (method.delay.Length != part)
			{
				method.delay = new float[part];
				method.decelerationStep = new float[part];
			}
			GUILayout.Label("");
			float decelerationSomme = 0;
			for (uint i = 0; i < method.delay.Length; i++)
			{
				GUILayout.Label("Part " + (i + 1));
				method.delay[i] = EditorGUILayout.FloatField("Duration", method.delay[i]);
				if (i < method.delay.Length - 1) method.decelerationStep[i] = Mathf.Clamp(EditorGUILayout.FloatField("Deceleration Step", method.decelerationStep[i]), 0, 1 - decelerationSomme);
				else method.decelerationStep[i] = EditorGUILayout.FloatField("Deceleration Step", 1 - decelerationSomme);
				decelerationSomme += method.decelerationStep[i];
				GUILayout.Label("");
			}
		}
	}

	public enum CurveMethods : int
	{ 
		Simplified,
		Advanced,
		Curve
	}
}