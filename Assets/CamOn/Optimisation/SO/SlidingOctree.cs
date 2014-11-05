using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CamOn.Properties;
using CamOn.Utils;

namespace CamOn.Optimisation.SO
{
	public class SlidingOctree : CameraOptimiser
	{
		private float scaleFactor = 0.75f;
		private int passes = 20;
		private float step = 1.0f;
		private float rotationStep = 10.0f;
		
		private int currentPass = 0;
		private List<Vector3> nextPositions;
		private List<Vector2> nextRotations;
		private bool random=false;
		private Vector3 min,max;
		
		internal float tmpFitness;

		#region implemented abstract members of CamOn.Optimisation.CameraOptimiser
		protected override void init ()
		{
			nextPositions = new List<Vector3> ();
			nextRotations = new List<Vector2> ();
			
			if (random) {
				Vector3 interval = max - min;
				Vector3 mean = 0.5f * (max + min);
				tmpCamera.transform.position = GeometryUtilityExtra.RandomValidPosition (mean, 0.5f * interval);
				mean = Vector3.zero;
				foreach (SubjectEvaluator s in currentCameraSubjects)
					mean += s.Center / currentCameraSubjects.Count;
				currentBest.transform.LookAt (mean);
			}
			
			CurrentBestCamera.transform.position = tmpCamera.transform.position;
			CurrentBestCamera.transform.rotation = tmpCamera.transform.rotation;
			currentBestFitness = EvaluateCamera (tmpCamera);
		}
		
		
		internal override void loop ()
		{
			if (nextPositions.Count == 0) {
				for (int x = -1; x <= 1; x += 2)
					for (int y = -1; y <= 1; y += 2)
						for (int z = -1; z <= 1; z += 2)
							for (int a = -1; a <= 1; a += 2)
								for (int b = -1; b <= 1; b += 2) {
									nextPositions.Add (CurrentBestCamera.transform.position +  Mathf.Pow(scaleFactor,currentPass) * step * (new Vector3 (x, y, z)));
									nextRotations.Add (CurrentBestCamera.transform.GetSphericalRotation () + Mathf.Pow (scaleFactor, currentPass) * rotationStep * (new Vector2 (a, b)));
								}
				if (++currentPass == passes)
					currentPass = 0;
			}
			tmpCamera.transform.position = nextPositions.Last ();
			tmpCamera.transform.SetSphericalRotation (nextRotations.Last ());
			tmpFitness = EvaluateCamera (tmpCamera);
			
			nextPositions.Remove (nextPositions.Last ());
			nextRotations.Remove (nextRotations.Last ());

			currentBestFitness = EvaluateCamera(CurrentBestCamera);
			if (tmpFitness > currentBestFitness) {
				CurrentBestCamera.transform.position = tmpCamera.transform.position;
				CurrentBestCamera.transform.rotation = tmpCamera.transform.rotation;
				currentBestFitness = tmpFitness;
			}
		}
		
		#endregion
		public SlidingOctree (Controller co) : base(co)
		{
		}
		
		public SlidingOctree (float scaleFactor, int passes, float step, Controller co) : base(co)
		{
			this.scaleFactor = scaleFactor;
			this.passes = passes;
			this.step = step;
			rotationStep = 10 * step;
		}
		
		/*
		 * Random initialisation constructor
		 */
		public SlidingOctree (float scaleFactor, int passes, float step, Vector3 min, Vector3 max, Controller co) : base(co)
		{
			this.scaleFactor = scaleFactor;
			this.passes = passes;
			this.step = step;
			rotationStep = 10 * step;
			random = true;
			this.min = min;
			this.max = max;
		}
	}
}

