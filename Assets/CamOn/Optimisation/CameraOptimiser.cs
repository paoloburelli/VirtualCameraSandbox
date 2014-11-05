using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CamOn.Utils;

namespace CamOn.Optimisation
{
	public abstract class CameraOptimiser
	{
		protected List<SubjectEvaluator> currentCameraSubjects {
			get {
				return SubjectEvaluator.SceneSubjects;
			}
		}
		private bool run = false;
		private Controller controller;
		protected Controller Controller {
			get { return controller; }
		}
		
		private int evaluationsPerSecond;
		public int EvaluationsPerSecond
		{
			get { return evaluationsPerSecond;}
		}
		
		public CameraOptimiser (Controller controller)
		{
			this.controller = controller;
		}
		
		float resources = 1;
		public float Resources {
			get { return resources; }
			set { resources = Mathf.Clamp01 (value); }
		}
		
		internal Camera tmpCamera;
		internal Camera currentBest;
		public Camera CurrentBestCamera
		{
			get { return currentBest;}
		}

		internal float currentBestFitness=0;
		public float CurrentBestCameraFitness
		{
			get { return currentBestFitness;}
		}

		public void Start ()
		{
			Start (1);
		}
		
		public void Start (float resources)
		{
			Reset ();
			this.run = true;
			this.resources = resources;
			controller.StartCoroutine (optimise ());
		}
		
		public void Stop ()
		{
			this.run = false;
			GameObject.Destroy (currentBest.gameObject);
		}
		
		public void Reset ()
		{
			GameObject camObject = null;
			while ((camObject = GameObject.Find("[" + this.GetType ().Name + "]")) != null)
				GameObject.DestroyImmediate (camObject);
			
			Camera prevBest = currentBest;
			camObject = new GameObject ("[" + this.GetType ().Name + "]");//+ "-" + UnityEngine.Random.value.ToString ("0.0000").Replace ("0.", "") + " Camera]");
			camObject.AddComponent<Camera> ();
			camObject.camera.enabled = false;
			camObject.camera.CopyFrom (Camera.main);
			currentBest = camObject.camera;
			
			camObject = null;
			while ((camObject = GameObject.Find("[" + this.GetType ().Name + "-tmp]")) != null)
				GameObject.DestroyImmediate (camObject);
			
			camObject = new GameObject ("[" + this.GetType ().Name + "-tmp]");//"-" + UnityEngine.Random.value.ToString ("0.0000").Replace ("0.", "") + " Camera]");
			camObject.AddComponent<Camera> ();
			camObject.camera.enabled = false;
			camObject.camera.CopyFrom (Camera.main);
			tmpCamera = camObject.camera;
			
			
			//currentCameraSubjects = (SubjectEvaluator[])GameObject.FindObjectsOfType (typeof(SubjectEvaluator));
			init ();
			if (prevBest != null)
				GameObject.Destroy (prevBest.gameObject);
		}
		
		private IEnumerator<YieldInstruction> optimise ()
		{
			if (Debug.isDebugBuild)
				Debug.Log ("Start " + this.GetType ().Name);
			
			float begin = Time.realtimeSinceStartup;
			int evaluationsPerSecond = 0;
			while (run) {
				evaluationsPerSecond += currentCameraSubjects.Count * (int)(1 / Time.deltaTime);
				loop ();
				if (Time.realtimeSinceStartup - begin >= resources / CamOnSettings.Instance.MinimumFramerate) {
					begin = Time.realtimeSinceStartup;
					yield return new WaitForEndOfFrame ();
					this.evaluationsPerSecond = evaluationsPerSecond;
					evaluationsPerSecond = 0;
				}
			}
			
			if (Debug.isDebugBuild)
				Debug.Log ("Stop " + this.GetType ().Name);
		}
		
		public float EvaluateCamera (Camera camera)
		{
			float val = 0;
			float enabledSubjectsPriority = 0;
			
			if (!GeometryUtilityExtra.IsValidPosition (camera.transform.position))
				return 0;

			foreach (SubjectEvaluator sub in GameObject.FindObjectsOfType (typeof(SubjectEvaluator))) {
				if (sub.Active && !sub.Ignore) {
					val += sub.EvaluateProperties (camera) * sub.Priority;
					enabledSubjectsPriority += sub.Priority;
				}
			}
			
			return val / enabledSubjectsPriority;
		}
		
		protected abstract void init ();
		internal abstract void loop ();
	}
}

