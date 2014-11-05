using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.Collections.Generic;
using CamOn;

public class CamOnConfiguration : EditorWindow
{
	const string RES_PATH = "Assets/CamOn/Resources/";
	bool seeCameraSettings=true, seeSolverSettings=true;
	CamOnSettings settings;
	
	[MenuItem("Window/CamOn Settings")]
	static void Init ()
	{
		EditorWindow.GetWindow (typeof(CamOnConfiguration));
	}
	
	void OnGUI ()
	{
		Type seettingsType = typeof(CamOnSettings);
		
		if (settings == null)
			settings = (CamOnSettings)AssetDatabase.LoadAssetAtPath (RES_PATH + seettingsType.Name + ".asset", seettingsType);
		if (settings == null) {
			settings = ScriptableObject.CreateInstance<CamOnSettings> ();
			AssetDatabase.CreateAsset (settings, RES_PATH + seettingsType.Name + ".asset");
		}
		
		seeCameraSettings = EditorGUILayout.Foldout (seeCameraSettings, "Camera");
		if (seeCameraSettings) {
			settings.Speed = EditorGUILayout.FloatField ("Max Speed", settings.Speed);
			settings.Reactivity = EditorGUILayout.Slider ("Reactivity", settings.Reactivity, 0.001f, 1);
		}
		seeSolverSettings = EditorGUILayout.Foldout (seeSolverSettings, "Solver");
		if (seeSolverSettings) {
			settings.MainSubject = (SubjectEvaluator)EditorGUILayout.ObjectField ("Main Subject", settings.MainSubject, typeof(SubjectEvaluator), true);
			settings.MinimumFramerate = EditorGUILayout.IntSlider ("Min Framerate", settings.MinimumFramerate, 1, 120);
			settings.DebugOnScreen = EditorGUILayout.Toggle ("Show Debug Info", settings.DebugOnScreen);
		}
		
		EditorUtility.SetDirty (settings);
	}
}
