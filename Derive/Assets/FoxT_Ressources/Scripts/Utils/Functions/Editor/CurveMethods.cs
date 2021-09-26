using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Fox.Editor
{
    public static class CurveOptions
    {
		private static CurveMethods currentPushMethod = CurveMethods.Simplified;
        public static void Display()
        {
			if (GUI.Button(new Rect(5, 200, Screen.width / 3 - 10, 25), "Simplified"))
			{
				currentPushMethod = CurveMethods.Simplified;
			}
			else if (GUI.Button(new Rect(5 + (Screen.width / 3), 200, Screen.width / 3 - 10, 25), "Advanced"))
			{
				currentPushMethod = CurveMethods.Advanced;
			}
			else if (GUI.Button(new Rect(5 + (Screen.width / 3) * 2, 200, Screen.width / 3 - 10, 25), "Curve"))
			{
				currentPushMethod = CurveMethods.Curve;
			}
		}

		public static string CurrentMethods()
		{
			return currentPushMethod.ToString();
		}
    }

	public enum CurveMethods
	{ 
		Simplified,
		Advanced,
		Curve
	}
}