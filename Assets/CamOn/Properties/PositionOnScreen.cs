using System;
using UnityEngine;

namespace CamOn.Properties {
	public class PositionOnScreen : Property{
		
		internal Vector2 screenPosition;
		public Vector2 DesiredScreenPosition {
			get {return screenPosition;}
		}
		
		public Vector2 ActualScreenPosition {
			get { return Target.PositionOnScreen; }
		}
		
		public PositionOnScreen (float x, float y)
		{
			screenPosition = new Vector2 (x, y);
		}
		
		public PositionOnScreen (float x, float y, float priority) : base(priority)
		{
			screenPosition = new Vector2 (x, y);
		}
		
		public PositionOnScreen (Vector2 screenPosition)
		{
			this.screenPosition = screenPosition;
		}

		public PositionOnScreen (Vector2 screenPosition, float priority) : base(priority)
		{
			this.screenPosition = screenPosition;
		}
		
		protected override float evaluate ()
		{
			return Mathf.Clamp01(1 - (screenPosition - Target.PositionOnScreen).magnitude/1.414f);
		}
	}
}