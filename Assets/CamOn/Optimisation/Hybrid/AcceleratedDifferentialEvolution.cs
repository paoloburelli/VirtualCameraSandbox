using System;
using UnityEngine;
using CamOn.Properties;
using CamOn.Utils;
using CamOn.Optimisation.APF;
using CamOn.Optimisation.DE;

namespace CamOn.Optimisation.Hybrid
{
	public class AcceleratedDifferentialEvolution : CameraOptimiser
	{
		public Vector3 CurrentLookAtPoint;
		DifferentialEvolution de;
		ArtificialPotentialField apf;
		
		private void normaliseLookAtPoint ()
		{
			Vector3 averagePosition = Vector3.zero;
			foreach (SubjectEvaluator sub in currentCameraSubjects) {
				if (sub.Active && !sub.Ignore)
					averagePosition += sub.Position / currentCameraSubjects.Count;
			}
			
			CurrentLookAtPoint = CurrentBestCamera.transform.position + CurrentBestCamera.transform.forward * (averagePosition - CurrentBestCamera.transform.position).magnitude;			
		}
		
		
		#region implemented abstract members of CamOn.Optimisation.CameraOptimiser
		protected override void init ()
		{
			de.Reset ();
			apf.Reset ();
		}
		
		float apfFitnees = 0.0f;
		int index = 0;
		int resources = 1;
		private bool NeedDE ()
		{
			bool returnValue = false;
			foreach (SubjectEvaluator s in currentCameraSubjects)
				if (s.ShoulBeVisible ())
					returnValue = true;
			
			apfFitnees = 0;
			foreach (SubjectEvaluator s in currentCameraSubjects) {
				apfFitnees += s.Satisfaction / currentCameraSubjects.Count;
				if (s.ShoulBeVisible() && s.VisibilitySatisfaction () > 0)
					returnValue = false;
			}
			return returnValue;
		}
		
		bool needDE = false;
		bool swapped = false;
		internal override void loop ()
		{
			if (swapped) {
				de.GlobalOptimum.fitness = apfFitnees;
				swapped = false;
			}
			
			int dynamicResource = needDE ? resources : 10 - resources;				

			if (index > dynamicResource)
				de.loop ();
			else {
				apf.loop ();
				needDE = NeedDE ();
			}
				
			if (needDE && de.GlobalOptimum.Fitness > apfFitnees) {
				swapped = true;
				needDE = false;
				index = 0;
				apf.currentBest.transform.position = de.GlobalOptimum.Position;
				apf.currentBest.transform.SetSphericalRotation (de.GlobalOptimum.Rotation);
				apf.normaliseLookAtPoint ();
			}
				
			currentBest = apf.CurrentBestCamera;
			currentBestFitness = apf.CurrentBestCameraFitness;
			
			if (index++ == 10)
				index = 0;
		}
		
		#endregion
		
		public AcceleratedDifferentialEvolution (float crossoverProbability, float weightingFactor, int popSize,Vector3 min, Vector3 max, bool randomStartPoint, Controller co) : base(co)
		{
			de = new DifferentialEvolution (crossoverProbability, weightingFactor, popSize, min, max, Controller);
			if (randomStartPoint)
				apf = new ArtificialPotentialField (min, max, Controller);
			else
				apf = new ArtificialPotentialField (Controller);
		}
	}
}

