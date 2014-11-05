using System;
using UnityEngine;

namespace CamOn.Utils {
public static class TransformExtension
	{
	    public static void PlaceAndRotateWithSphericalCoordinates (this Transform transform, Vector3 position, Vector2 rotation)
	    {
	    	transform.position = position;
	    	transform.SetSphericalRotation (rotation);
	    }
	
	    public static Vector2 GetSphericalRotation (this Transform transform)
	    {
	    	return transform.forward.GetSphericalRotation ();
	    }	
		
		public static Vector2 GetSphericalRotation (this Vector3 forward)
		{
			float h = Mathf.Atan2 (forward.x, forward.z) * 180 / Mathf.PI;
			
			if (h > 180)
				h -= 360;
			
			if (h < -180)
				h += 180;
			
			float v = Mathf.Asin (forward.y/forward.magnitude) * 180 / Mathf.PI;
			return new Vector2 (h, v);
		}
		
		public static void SetSphericalRotation (this Transform transform, Vector2 rotation)
		{
			rotation.y = Mathf.Clamp (rotation.y, -90, 90);
			rotation.x = Mathf.Clamp (rotation.x, -180, 180);
			transform.rotation = Quaternion.Euler (-rotation.y, rotation.x, 0);
		}
		
		
		public static Vector3 Rotate (this Vector3 vector, Vector2 rotation)
		{
			return vector.Rotate (rotation.x, rotation.y);
		}
		
		public static Vector3 Rotate (this Vector3 vector, float h, float v)
		{
			Vector2 rot = vector.GetSphericalRotation ();
			return Quaternion.Euler (-v + rot.y, h + rot.x, 0) * Vector3.forward * vector.magnitude;
		}
	}
}