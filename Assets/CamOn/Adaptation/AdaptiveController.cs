using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CamOn.Optimisation;
using CamOn.Optimisation.APF;
using CamOn.Utils;

using CamOn.Properties;

namespace CamOn.Adaptation {
	public abstract class AdaptiveController : CamOn.Controller
	{
		public List<CameraBehaviour> cameraBehaviours;
	
		// Use this for initialization
		public new void Start ()
		{
			base.Start ();
			cameraBehaviours = new List<CameraBehaviour> ();
		}
		
		protected abstract CameraBehaviour updateCurrentBehaviour();
		
		private CameraBehaviour currentBehaviour;
		public CameraBehaviour CurrentBehaviour {
			get {
				if (currentBehaviour == null)
					UpdateCurrentBehaviour ();
				return currentBehaviour;
			}
		}
		public void UpdateCurrentBehaviour ()
		{
			currentBehaviour = updateCurrentBehaviour ();
		}
	}
}