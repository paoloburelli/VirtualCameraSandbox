using UnityEngine;
using UnityEditor;
using System.Collections;
using CamOn;
using CamOn.Properties;
using CamOn.Utils;

[CustomEditor(typeof(SubjectEvaluator))]
public class SubjectVisualiser : Editor
{
	SubjectEvaluator subject;
	
	public override void OnInspectorGUI ()
	{
		subject = (SubjectEvaluator)target;
		
		EditorGUILayout.LabelField ("Visibility", subject.Visibility.ToString ("0.00"));
		EditorGUILayout.LabelField ("In Frame Fraction", subject.OnScreenFraction.ToString ("0.00"));
		string occlusionString = "";
		
		if (subject.AllUnoccluded)
			occlusionString = "None";
		else if (subject.AllOccluded)
			occlusionString = "Complete";
		else {
			if (subject.TopOccluded)
				occlusionString += "Top,";
			if (subject.BottomOccluded)
				occlusionString += "Bottom,";
			if (subject.LeftOccluded)
				occlusionString += "Left,";
			if (subject.RightOccluded)
				occlusionString += "Right,";
			if (subject.CenterOccluded)
				occlusionString += "Center,";
		}
		
		occlusionString = occlusionString.TrimEnd (',');
		EditorGUILayout.LabelField ("Occlusion", occlusionString);
		EditorGUILayout.LabelField ("Occluders", subject.Occluders);
		EditorGUILayout.LabelField ("Angle", subject.CameraSphericalAngles.ToString ("0"));
		EditorGUILayout.LabelField ("Projection Size", subject.ProjectionSize.ToString ("0.00"));
		EditorGUILayout.LabelField ("Position On Screen", subject.PositionOnScreen.ToString ("0.00"));
		EditorGUILayout.LabelField ("Satisfaction", subject.Satisfaction.ToString ("0.00"));
	}
	
	public void OnSceneGUI ()
	{
		subject = (SubjectEvaluator)target;
		//subject.CameraBoundsRenderer.enabled = true && CamOnSettings.Instance.DebugOnScreen;
	}
}

