using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CamOn.Optimisation;
using CamOn.Optimisation.APF;
using CamOn.Utils;

namespace CamOn
{
	public class Controller : MonoBehaviour
	{
		internal static bool MainSubjectOccluded = false;
		
		public float CurrentScore = 1.0f;
		ArtificialPotentialField co;
		List<Vector3> positions = new List<Vector3> ();
		List<Vector3> lookAtPoints = new List<Vector3> ();

		public int EvaluationsPerSecond {
			get { return co != null ? co.EvaluationsPerSecond : 0; }
		}

		private static int buffersize = 10;

		public void Start ()
		{
			co = new ArtificialPotentialField (this);
			co.Start ();
		}

		public void Reset ()
		{
			if (enabled) {
				co.Reset ();
				positions.Clear ();
				lookAtPoints.Clear ();
			}
		}
		
		public void Update ()
		{
			buffersize = Mathf.FloorToInt (1 / CamOnSettings.Instance.Reactivity);
			CurrentScore = co.EvaluateCamera (gameObject.camera);
			if (Debug.isDebugBuild && Time.frameCount % 15 == 0 && CamOnSettings.Instance.DebugOnScreen)
				Label.OnScreen.Write ("Current shot score: " + (CurrentScore).ToString ("0.00") + 
					"\nEvaluations per second: " + EvaluationsPerSecond + 
					"\nFPS:" + (Time.timeScale / Time.deltaTime).ToString ("0") + 
					"\nMax Speed: " + CamOnSettings.Instance.Speed + 
					"\nReactivity: " + CamOnSettings.Instance.Reactivity + 
					"\nMain Subject: " + CamOnSettings.Instance.MainSubject
					);
			
			if (!CamOnSettings.Instance.DebugOnScreen)
				Label.OnScreen.Write ("");
			
		}
		
		public void LateUpdate ()
		{			
			if (MainSubjectOccluded){

				if (Debug.isDebugBuild)
					Debug.Log("Rescue!");
				positions.Clear();
				lookAtPoints.Clear();
				float maxDistance = (gameObject.camera.transform.position-CamOnSettings.Instance.MainSubject.transform.position).magnitude/2;
				gameObject.camera.transform.position += Mathf.Clamp(CamOnSettings.Instance.Speed*Time.smoothDeltaTime,0,maxDistance)*(CamOnSettings.Instance.MainSubject.transform.position - gameObject.camera.transform.position).normalized;
				co.Reset();
				
			} else {
				positions.Add (co.CurrentBestCamera.transform.position);
			
				while (positions.Count > buffersize && positions.Count>0) {
					positions.RemoveAt (0);
				}
				Vector3 targetPosition = Vector3.zero;
				foreach (Vector3 v in positions)
					targetPosition += v / positions.Count;
				
				
				Vector3 offset = targetPosition-gameObject.camera.transform.position;
				gameObject.camera.transform.position += offset.magnitude <= CamOnSettings.Instance.Speed*Time.smoothDeltaTime ? offset : offset.normalized*CamOnSettings.Instance.Speed*Time.smoothDeltaTime;
				
				lookAtPoints.Add (co.CurrentLookAtPoint);
				while (lookAtPoints.Count > buffersize && lookAtPoints.Count>0) {
					lookAtPoints.RemoveAt (0);
				}
				Vector3 targetLookAtPoint = Vector3.zero;
				foreach (Vector3 v in lookAtPoints)
					targetLookAtPoint += v / lookAtPoints.Count;
				
				gameObject.camera.transform.LookAt (targetLookAtPoint, Vector3.up);
			}
		}

		public void OnDrawGizmosSelected  ()
		{
			if (Application.isPlaying && enabled) {
				Gizmos.DrawCube (co.CurrentLookAtPoint,Vector3.one * 0.1f);
				Gizmos.DrawLine (camera.transform.position, co.CurrentLookAtPoint);
			}
		}
	}
}
