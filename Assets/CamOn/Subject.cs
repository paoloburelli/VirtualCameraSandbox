using UnityEngine;
using System.Collections;
using CamOn;
using CamOn.Properties;
using CamOn.Utils;

[AddComponentMenu("Camera-Control/CamOn Subject")]
[RequireComponent(typeof(SubjectEvaluator))]
public class Subject : MonoBehaviour
{
	public enum AngleType { None, Horizontal, Vertical, Both};
	public AngleType EnableAngle = AngleType.None;
	public Vector2 DesiredAngle = Vector2.zero;
	public float AnglePriority = 1;
	public bool EnableProjectionSize=false;
	public float DesiredSize=1,SizePriority=1;
	public bool EnablePositionOnScreen=false;
	public Vector2 DesiredPositionOnscreen=new Vector2(0.5f,0.5f);
	public float PositionOnScreenPriority=1;
	public float DesiredVisibility=1;
	public float VisibilityPriority=1;
	
	public PrimitiveType CameraBoundsType = PrimitiveType.Capsule;
	public Vector3 CameraBoundsOffset = Vector3.zero;
	public Quaternion CameraBoundsRotation = Quaternion.identity;
	public Vector3 CameraBoundsScale = Vector3.one;
	
	public void updateObjectiveFunction ()
	{
		SubjectEvaluator se = GetComponent<SubjectEvaluator> ();
		se.ClearAllProperties ();
		se.AddProperty (new Visibility (DesiredVisibility, VisibilityPriority));
		if (DesiredVisibility > 0) {
			if (EnableAngle == AngleType.Both)
				se.AddProperty (new Angle (DesiredAngle, AnglePriority));
			if (EnableAngle == AngleType.Horizontal)
				se.AddProperty (new HorizontalAngle (DesiredAngle.x,AnglePriority));
			if (EnableAngle == AngleType.Vertical)
				se.AddProperty (new VerticalAngle (DesiredAngle.y,AnglePriority));
			if (EnableProjectionSize)
				se.AddProperty (new ProjectionSize (DesiredSize, SizePriority));
			if (EnablePositionOnScreen)
				se.AddProperty (new PositionOnScreen (DesiredPositionOnscreen, PositionOnScreenPriority));
		}
	}
	
	public void updateCameraBounds ()
	{
		
		GetComponent<SubjectEvaluator> ().CameraBoundType = CameraBoundsType;
		GetComponent<SubjectEvaluator> ().CameraBoundsRotation = CameraBoundsRotation;
		GetComponent<SubjectEvaluator> ().CameraBoundsCenter = CameraBoundsOffset;
		GetComponent<SubjectEvaluator> ().CameraBoundsScale = CameraBoundsScale;
	}
	
	public void Start ()
	{
		updateCameraBounds ();
		updateObjectiveFunction ();
		if (Camera.main.GetComponent<AutomaticController> () == null)
			Camera.main.gameObject.AddComponent<AutomaticController> ();
		Camera.main.GetComponent<AutomaticController> ().enabled = true;
	}
}
