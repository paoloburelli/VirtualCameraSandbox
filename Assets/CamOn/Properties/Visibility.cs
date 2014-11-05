using System;
using System.Collections.Generic;
using UnityEngine;
using CamOn.Utils;

namespace CamOn.Properties {
	public class Visibility : Property
	{
		public float DesiredVisibility {
			get { return desiredVisibility;}
		}
		
		public float ActualVisibility {
			get { return Target.Visibility; }
		}
		
		float desiredVisibility = 0;
		public Visibility (float desiredVisibility)
		{
			this.desiredVisibility = desiredVisibility;
		}
		
		public Visibility (float desiredVisibility, float priority) : base(priority)
		{
			this.desiredVisibility = desiredVisibility;
		}
		
		protected override float evaluate ()
		{
			return 1 - Mathf.Abs (desiredVisibility - Target.Visibility);
		}
	}
}