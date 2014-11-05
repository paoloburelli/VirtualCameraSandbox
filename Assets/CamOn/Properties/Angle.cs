using UnityEngine;
using System;
using CamOn.Utils;

namespace CamOn.Properties {
	public class Angle : Property
	{
		protected Vector2 desiredAngle;
		public virtual Vector2 DesiredAngle {
			get { return desiredAngle; }
		}
		
		public Vector3 DesiredCameraDirection {
			get {
				return Target.Forward.Rotate (DesiredAngle); 
			}

		}
		
		public Angle (Vector2 angle)
		{
			this.desiredAngle = angle;
		}
		
		public Angle (float horizontal, float vertical)
		{
			this.desiredAngle = new Vector2(horizontal,vertical);
		}
		
		public Angle (Vector2 angle, float priority) : base(priority)
		{
			this.desiredAngle = angle;
		}
		
		public Angle (float horizontal, float vertical, float priority) : base(priority)
		{
			this.desiredAngle = new Vector2 (horizontal, vertical);
		}
	
		protected override float evaluate ()
		{
			//Vector2 vantageAngle = new Vector2();
			//vantageAngle.y = Mathf.Asin(Target.RelativeCameraPosition.normalized.y) * Mathf.Rad2Deg;
			//vantageAngle.x = -Mathf.Atan2(Target.RelativeCameraPosition.normalized.x,Target.RelativeCameraPosition.normalized.z) * Mathf.Rad2Deg;

			//Debug.Log(Target.RelativeCameraPosition + ": "+ vantageAngle);

			float ax = Target.CameraSphericalAngles.x%360;
			if (ax < 0)
				ax += 360;

			float bx = desiredAngle.x%360;
			if (bx < 0)
				bx += 360;


			float hAngleDifference = Mathf.Abs(ax - bx);
			if (hAngleDifference > 180)
				hAngleDifference = 360-hAngleDifference;
			
			float hSatisfaction = 1-hAngleDifference/180;




			float ay = Target.CameraSphericalAngles.y%180;
			if (ay < 0)
				ay += 180;
			
			float by = desiredAngle.y%180;
			if (by < 0)
				by += 180;


			float vSatisfaction = 1-(Mathf.Abs(ay - by))/180;


			//Debug.Log(hSatisfaction*vSatisfaction);
			return (hSatisfaction*vSatisfaction);

			//return 1 - Mathf.Abs (Target.CameraAngle - Vector3.Angle(Vector3.forward,Vector3.forward.Rotate(DesiredAngle))) / 180;
		}
	}
	
	public class HorizontalAngle : Angle {
		
		public HorizontalAngle (float angle) : base(angle, 0){}
		public HorizontalAngle (float angle,float priority) : base(angle,0,priority){}
		
		public override Vector2 DesiredAngle {
			get { return new Vector2(desiredAngle.x, Target.CameraSphericalAngles.y); }
		}
		
		protected override float evaluate ()
		{
			return 1 - Mathf.Abs (Target.CameraSphericalAngles.x - desiredAngle.x) / 180;
		}
	}
	
	public class VerticalAngle : Angle
	{

		public VerticalAngle (float angle) : base(0, angle){}
		public VerticalAngle (float angle, float priority) : base(0, angle, priority){}

		public override Vector2 DesiredAngle {
			get { return new Vector2 (Target.CameraSphericalAngles.x, desiredAngle.y); }
		}

		protected override float evaluate ()
		{
			return 1 - Mathf.Abs (Target.CameraSphericalAngles.y - desiredAngle.y) / 180;
		}
	}
}