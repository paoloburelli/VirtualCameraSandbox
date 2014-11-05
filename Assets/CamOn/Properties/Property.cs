using UnityEngine;
using System;

namespace CamOn.Properties {
	public abstract class Property
	{
		public SubjectBackend Target = null;
		protected float priority = 1.0f;
	
		public float Satisfaction {
			get {
				if (Target != null)
					return evaluate ();
				return 1.0f;
			}
		}
		
		public float Priority {
			get { return priority; }
		}
		
		public Property (float priority)
		{
			this.priority = Mathf.Clamp01 (priority);			
		}		
		
		public Property ()
		{
			
		}
		
		public Property Clone ()
		{
			return (Property)this.MemberwiseClone ();
		}
		
		protected abstract float evaluate();
	}
}