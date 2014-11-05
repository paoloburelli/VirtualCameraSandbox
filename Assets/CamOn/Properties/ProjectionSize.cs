using UnityEngine;
using System.Collections;
using CamOn.Utils;

namespace CamOn.Properties {
	public class ProjectionSize : Property
	{	
	    protected float desiredSize;
		
		public float DesiredSize{
			get {return desiredSize;}
		}
	
		public float ActualSize {
			get { return Target.ProjectionSize;}
		}
		
		public ProjectionSize (float desiredSize)
		{
			this.desiredSize = desiredSize;
		}
		
		public ProjectionSize (float desiredSize, float priority) : base(priority)
		{
			this.desiredSize = desiredSize;
		}
		
		protected override float evaluate ()
		{
			return Mathf.Clamp01(1-(Mathf.Abs(DesiredSize - ActualSize)/DesiredSize));
		}
	}
}