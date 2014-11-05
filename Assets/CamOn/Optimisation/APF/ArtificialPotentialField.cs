using System;
using UnityEngine;
using CamOn.Properties;
using CamOn.Utils;

namespace CamOn.Optimisation.APF
{
	public class ArtificialPotentialField : CameraOptimiser
	{
		public Vector3 CurrentLookAtPoint;
		
		internal void normaliseLookAtPoint ()
		{
			Vector3 averagePosition = Vector3.zero;
			foreach (SubjectEvaluator sub in currentCameraSubjects) {
				if (sub != null && sub.Active && !sub.Ignore)
					averagePosition += sub.Position / currentCameraSubjects.Count;
			}
			
			CurrentLookAtPoint = CurrentBestCamera.transform.position + CurrentBestCamera.transform.forward * (averagePosition - CurrentBestCamera.transform.position).magnitude;			
		}
		
		
		#region implemented abstract members of CamOn.Optimisation.CameraOptimiser
		protected override void init ()
		{
			normaliseLookAtPoint ();
			if (random) {
				Vector3 interval = randomMax - randomMin;
				Vector3 mean = 0.5f * (randomMax + randomMin);
				currentBest.transform.position = GeometryUtilityExtra.RandomValidPosition (mean, 0.5f * interval);
				
				mean = Vector3.zero;
				foreach (SubjectEvaluator s in currentCameraSubjects)
					mean += s.Center / currentCameraSubjects.Count;
				currentBest.transform.LookAt (mean);
			}
		}
		
		
		internal override void loop ()
		{
			Vector3 newPosition = CurrentBestCamera.transform.position;
			Vector3 lookAtShift = Vector3.zero;
			
			foreach (SubjectEvaluator sub in currentCameraSubjects) {
				
				if (sub != null && sub.Active && !sub.Ignore) {
					sub.EvaluateProperties (CurrentBestCamera);
				
					bool visibilityCondition = sub.VisibilitySatisfaction () > 0.10f;
					
					foreach (Property p in sub) {
						if (p is Visibility || visibilityCondition) {
							newPosition += p.PositionForce (currentBest) * 0.05f;
							lookAtShift += p.ViewpointForce (currentBest, CurrentLookAtPoint) * 0.3f;
						}
					}
				}
			}
			
			newPosition += PropertiesForces.VisibilityPositionForce (currentCameraSubjects, currentBest) * 0.1f;
			
			CurrentLookAtPoint += lookAtShift;
			if (!float.IsNaN(newPosition.x) && !float.IsNaN(CurrentLookAtPoint.x)){
				CurrentBestCamera.transform.position = newPosition;

				CurrentBestCamera.transform.LookAt (CurrentLookAtPoint, Vector3.up);

				currentBestFitness = EvaluateCamera(CurrentBestCamera);

				
				normaliseLookAtPoint ();
			}
		}
		
		#endregion
		public ArtificialPotentialField (Controller co) : base(co)
		{
		}
		
		private bool random = false;
		private Vector3 randomMin,randomMax;
		public ArtificialPotentialField (Vector3 min, Vector3 max, Controller co) : base(co)
		{
			randomMin = min;
			randomMax = max;
			random = true;
		}
	}
}

