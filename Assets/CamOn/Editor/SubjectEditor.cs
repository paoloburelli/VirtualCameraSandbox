using UnityEngine;
using UnityEditor;
using System.Collections;
using CamOn;
using CamOn.Properties;
using CamOn.Utils;

[CustomEditor (typeof(Subject))]
public class SubjectEditor : Editor
{	
	static bool subjectsettings=true;
	static bool setup=true;
	
	Subject subject;

	public void OnEnable ()
	{
		subject = (Subject)target;
		SubjectEvaluator se = subject.GetComponent<SubjectEvaluator> ();
		if (!Application.isPlaying && se != null && se.CameraBoundsRenderer != null && 
			(se.CameraBoundsCenter != Vector3.zero || se.CameraBoundsRotation != Quaternion.identity ||
				se.CameraBoundsScale != Vector3.one || se.CameraBoundType != PrimitiveType.Capsule)) {
				subject.CameraBoundsType = se.CameraBoundType;
				subject.CameraBoundsOffset = se.CameraBoundsCenter;
				subject.CameraBoundsRotation = se.CameraBoundsRotation;
				subject.CameraBoundsScale = se.CameraBoundsScale;
			}
			subject = (Subject)target;
			subject.updateObjectiveFunction ();
			subject.updateCameraBounds ();
	}
	
	public override void OnInspectorGUI ()
	{
		subject = (Subject)target;
		
		setup = EditorGUILayout.Foldout (setup, "Visual Properties");
		if (setup) {
			subject.DesiredVisibility = EditorGUILayout.IntSlider ("Desired Visibility", (int)subject.DesiredVisibility, 0, 1, null);
			subject.VisibilityPriority = EditorGUILayout.Slider ("Visibility Priority", subject.VisibilityPriority, 0, 1, null);
		
			EditorGUILayout.Separator ();
		
			subject.EnableAngle = (Subject.AngleType)EditorGUILayout.EnumPopup ("Angle", subject.EnableAngle);
			switch (subject.EnableAngle) {
			case Subject.AngleType.Both:
				subject.DesiredAngle = EditorGUILayout.Vector2Field ("Desired Values", subject.DesiredAngle);
				break;
			case Subject.AngleType.Horizontal:
				subject.DesiredAngle.x = EditorGUILayout.FloatField ("Desired Value", subject.DesiredAngle.x);
				break;
			case Subject.AngleType.Vertical:
				subject.DesiredAngle.y = EditorGUILayout.FloatField ("Desired Value", subject.DesiredAngle.y);
				break;
			}
			if (subject.EnableAngle != Subject.AngleType.None) {
				subject.AnglePriority = EditorGUILayout.Slider ("Angle Priority", subject.AnglePriority, 0, 1, null);
			}
		
				EditorGUILayout.Separator ();
		
				subject.EnableProjectionSize = EditorGUILayout.BeginToggleGroup ("Projection Size", subject.EnableProjectionSize);
			subject.DesiredSize = EditorGUILayout.FloatField ("Desired Size", subject.DesiredSize);
			subject.SizePriority = EditorGUILayout.Slider ("Size Priority", subject.SizePriority, 0, 1, null);
			EditorGUILayout.EndToggleGroup ();
		
				EditorGUILayout.Separator ();
		
				subject.EnablePositionOnScreen = EditorGUILayout.BeginToggleGroup ("Position In Frame", subject.EnablePositionOnScreen);
			subject.DesiredPositionOnscreen = EditorGUILayout.Vector2Field ("Desired Values", subject.DesiredPositionOnscreen);
			subject.PositionOnScreenPriority = EditorGUILayout.Slider ("Position Priority", subject.PositionOnScreenPriority, 0, 1, null);
			EditorGUILayout.EndToggleGroup ();
			
			subject.DesiredAngle.x = Mathf.Clamp (subject.DesiredAngle.x, -180, 180);
			subject.DesiredAngle.y = Mathf.Clamp (subject.DesiredAngle.y, -90, 90);
			if (subject.DesiredSize <= 0.01f)
				subject.DesiredSize = 0.01f;
			subject.DesiredPositionOnscreen.x = Mathf.Clamp01 (subject.DesiredPositionOnscreen.x);
			subject.DesiredPositionOnscreen.y = Mathf.Clamp01 (subject.DesiredPositionOnscreen.y);
		
		}
		
		subjectsettings = EditorGUILayout.Foldout (subjectsettings, "Subject Settings");
		if (subjectsettings) {
			subject.CameraBoundsType = (PrimitiveType)EditorGUILayout.EnumPopup ("Bounds Type", subject.CameraBoundsType);
			subject.CameraBoundsOffset = EditorGUILayout.Vector3Field ("Bounds Position", subject.CameraBoundsOffset);
			subject.CameraBoundsRotation = Quaternion.Euler (EditorGUILayout.Vector3Field ("Bounds Rotation", subject.CameraBoundsRotation.eulerAngles));
			subject.CameraBoundsScale = EditorGUILayout.Vector3Field ("Bounds Scale", subject.CameraBoundsScale);
		}
		
		if (GUI.changed) {
			subject.updateObjectiveFunction ();
			subject.updateCameraBounds ();
		}
	}
	
	[MenuItem("CONTEXT/Transform/Make It A CamOn Subject")]
	static void addCamon (MenuCommand command)
	{
		Transform tr = (Transform)command.context;
		if (tr.gameObject.GetComponent<Subject> () == null)
			tr.gameObject.AddComponent<Subject> ();
	}

	[MenuItem("CONTEXT/Transform/Rmove CamOn Subject")]
	static void removeCamon (MenuCommand command)
	{
		Transform tr = (Transform)command.context;
		if (tr.gameObject.GetComponent<Subject> () != null) {
			DestroyImmediate (tr.gameObject.GetComponent<Subject> ());
			DestroyImmediate (tr.gameObject.GetComponent<SubjectEvaluator> ());
		}
	}
}

