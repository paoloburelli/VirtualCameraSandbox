using UnityEngine;
using System.Collections;
using CamOn;
using CamOn.Properties;
using CamOn.Utils;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SubjectEvaluator : CamOn.SubjectBackend
{
	public float Satisfaction = 1.0f;
	public static List<SubjectEvaluator> SceneSubjects;
	public static Vector3 AllSubjectsCenter {
		get {
			Vector3 center = Vector3.zero;
			//Debug.Log(SceneSubjects.Count);
			foreach (SubjectEvaluator s in SceneSubjects)
				center += s.Position / SceneSubjects.Count;
			return center;
		}
	}
	
	public void Awake ()
	{
		if (SceneSubjects == null)
			SceneSubjects = new List<SubjectEvaluator>();
		SceneSubjects.Clear();

		SubjectEvaluator[] se =(SubjectEvaluator[])GameObject.FindObjectsOfType (typeof(SubjectEvaluator));		

		if (se == null){
			SceneSubjects.Add (this);
		} else
			foreach (SubjectEvaluator s in se)
				SceneSubjects.Add(s);
	}
	
	public void Update ()
	{
		if (CameraBoundsRenderer)
			CameraBoundsRenderer.enabled = (CameraBoundsRenderer.enabled || Application.isPlaying) && CamOnSettings.Instance.DebugOnScreen && Debug.isDebugBuild && Active && !Ignore;
		
		if (Active && !Ignore) {
			Satisfaction = base.EvaluateProperties (Camera.main);
			if (CamOnSettings.Instance.MainSubject == this || GameObject.FindObjectsOfType (typeof(SubjectEvaluator)).Length == 1) {
				CamOnSettings.Instance.MainSubject = this;
				Controller.MainSubjectOccluded = this.AllOccluded;
			}
		}
	}
		
	public void OnDrawGizmos ()
	{
		if (CameraBoundsRenderer != null && Debug.isDebugBuild && !Ignore && Active && CamOnSettings.Instance.DebugOnScreen) {
			
			Vector3[] vertices = { Top, Bottom, Left, Right, Center };
			if (OnScreenFraction == 0) {
				vertices[0] = CameraBoundsRenderer.bounds.center + Camera.main.transform.up * CameraBoundsRenderer.bounds.extents.magnitude / 2;
				vertices[1] = CameraBoundsRenderer.bounds.center - Camera.main.transform.up * CameraBoundsRenderer.bounds.extents.magnitude / 2;
				vertices[2] = CameraBoundsRenderer.bounds.center - Camera.main.transform.right * CameraBoundsRenderer.bounds.extents.magnitude / 2;
				vertices[3] = CameraBoundsRenderer.bounds.center + Camera.main.transform.right * CameraBoundsRenderer.bounds.extents.magnitude / 2;
				vertices[4] = CameraBoundsRenderer.bounds.center;
			}
			
			float sphereSize = (vertices[0] - vertices[1]).magnitude / 20;
			Gizmos.color = Color.white;
			if (TopOccluded) Gizmos.DrawWireSphere (vertices[0], sphereSize); else Gizmos.DrawSphere (vertices[0], sphereSize);
			Gizmos.color = Color.red;
			if (BottomOccluded)
				Gizmos.DrawWireSphere (vertices[1], sphereSize);
			else
				Gizmos.DrawSphere (vertices[1], sphereSize);
			
			Gizmos.color = Color.blue;
			if (LeftOccluded)
				Gizmos.DrawWireSphere (vertices[2], sphereSize);
			else
				Gizmos.DrawSphere (vertices[2], sphereSize);
			
			Gizmos.color = Color.green;
			if (RightOccluded)
				Gizmos.DrawWireSphere (vertices[3], sphereSize);
			else
				Gizmos.DrawSphere (vertices[3], sphereSize);
			
			Gizmos.color = Color.black;
			if (CenterOccluded)
				Gizmos.DrawWireSphere (vertices[4], sphereSize);
			else
				Gizmos.DrawSphere (vertices[4], sphereSize);
			
			Gizmos.color = Color.yellow;
			
			Gizmos.DrawLine (Position, Position + Forward * sphereSize * 20);
			Gizmos.DrawWireCube (Position, Vector3.one * sphereSize);
			
			if (GetProperty<Angle> () != null) {
				Gizmos.color = Color.magenta;
				Gizmos.DrawLine (Position, Position + GetProperty<Angle> ().DesiredCameraDirection * sphereSize * 20);
			}
		}
	}

	public void OnGUI ()
	{
		if (Debug.isDebugBuild && !Ignore && Active && CamOnSettings.Instance.DebugOnScreen) {
			try {
				if (ScreenLeft > 0 && 
					ScreenTop > 0 && 
					ScreenRight < Screen.width && 
					ScreenBottom < Screen.height &&
					ScreenRight > ScreenLeft &&
					ScreenBottom > ScreenTop)
					GUI.Box (new Rect (ScreenLeft * Screen.width, ScreenTop * Screen.height, Screen.width * (ScreenRight - ScreenLeft), Screen.height * (ScreenBottom - ScreenTop)), "Position\n" + PositionOnScreen.ToSandboxString () + "\n\nSize\n" + ProjectionSize.ToString ("0.00") + "\n\nVisibility\n" + Visibility.ToString ("0.00") + "\n\nAngle\n" + CameraSphericalAngles.ToSandboxString ());
			} catch {
			}
		}
	}
	
	private bool destroyed = false;
	public void Destroy ()
	{
		destroyed = true;
		GameObject.Destroy (this);
	}
	public bool Active {
		get { return !destroyed && gameObject != null && gameObject.activeSelf ; }
	}
}
