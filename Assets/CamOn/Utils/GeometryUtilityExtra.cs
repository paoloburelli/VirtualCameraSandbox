using System;
using UnityEngine;
using System.Collections.Generic;

namespace CamOn.Utils {
	public class GeometryUtilityExtra
	{
		private UnityEngine.Object[] sceneObjects;
		private float dataTimeStamp;
		private UnityEngine.Object[] SceneObjects {
			get {
				if (Time.timeSinceLevelLoad - dataTimeStamp > 1)
					UpdateData ();
				return sceneObjects;
			}
		}
		
		private void UpdateData ()
		{
			dataTimeStamp = Time.timeSinceLevelLoad;
			sceneObjects = UnityEngine.GameObject.FindObjectsOfType (typeof(Renderer));
			Renderer r0 = (Renderer)SceneObjects[0];
			b = new Bounds (r0.bounds.center, r0.bounds.extents);
			foreach (Renderer r in SceneObjects)
				b.Encapsulate (r.bounds);
		}
		
		private Bounds b;
		private static GeometryUtilityExtra instance=null;
		private static GeometryUtilityExtra Instance {
			get {
				if (instance == null) {
					instance = new GeometryUtilityExtra ();
				}
				return instance;
			}
		}
		
		public static Bounds Bounds {
			get { return Instance.b; }
		}
		
		private GeometryUtilityExtra ()
		{
			UpdateData ();
		}
	
//		NEEDS SOME MORE WORK		
//		public static Vector3 RandomValidPlacement (Bounds movableoOjectBounds, Vector3 center, Vector3 size)
//		{
//			Vector3 extents = movableoOjectBounds.extents;
//			Vector3 max = center + size * 0.5f;
//			Vector3 min = center - size * 0.5f;
//			
//			Vector3 rp = Random.GaussianVector3 (center, size * 0.3f);
//			
//			while (!IsValidPlacement (rp,extents) || rp.x < min.x || rp.x > max.x || rp.z < min.z || rp.z > max.z)
//				rp = Random.GaussianVector3 (center, size * 0.3f);
//			return rp;
//		}
		
		public static Vector3 RandomValidPosition (Vector3 center, Vector3 size)
		{
			Vector3 max = center + size * 0.5f;
			Vector3 min = center - size * 0.5f;
			Vector3 rp = Random.GaussianVector3 (center, size * 0.5f);
			int countdown = 10000;
			while ((!IsValidPosition (rp) || rp.x < min.x || rp.x > max.x || rp.z < min.z || rp.z > max.z) && countdown-- > 0) {
				rp = Random.GaussianVector3 (center, size * 0.5f);
			}
			return rp;
		}
		
		public static bool IsValidPlacement (Vector3 position, Vector3 extents)
		{
			if (!IsValidPosition (position))
				return false;
			
			for (int x = -1; x<2; x+=2)
				for (int y = -1; y<2; y+=2)
					for (int z = -1; z<2; z+=2) {
						Vector3 tmp = extents;
						tmp.Scale (new Vector3 (x, y, z));
						if (!IsValidPosition (position + 0.5f * tmp))
							return false;
					}
			
			return true;
		}
		
	    public static bool IsValidPosition (Vector3 position)
		{
			foreach (Renderer r in Instance.SceneObjects)
				if (r != null && r.collider != null && !r.collider.isTrigger && 
					r.gameObject.layer != 2 && r.gameObject.layer != 1 && 
					r.bounds.Contains (position)) {
				
					Ray ray = new Ray (position, (r.transform.position - position).normalized);
					float distance = (r.transform.position - position).magnitude;
					RaycastHit info;
					if (!Physics.Raycast (ray, out info, distance)) {
						ray = new Ray (position, (position - r.transform.position).normalized);
						distance = (r.collider.ClosestPointOnBounds (position) - position).magnitude;
						if (distance > 0 && Physics.Raycast (ray, out info, distance) && info.collider == r.collider)
							return false;
					}
				}
			return Bounds.Contains (position);
		}
		
//	     public static Vector3 AvoidCollisions (Vector3 position, Vector3 startingPosition, float cameraSize)
//	     {
//	     	Vector3 direction = position - startingPosition;
//	     	RaycastHit info = new RaycastHit ();
//	     	if (!Physics.SphereCast (startingPosition, cameraSize, direction.normalized, out info, (position - startingPosition).magnitude))
//	     		return position;
//	     	else {
//				float distance = info.distance;
//				Vector3 collisionPoint = startingPosition + direction.normalized * distance;
//				Vector3 collisionSurfaceNormal = info.normal;
//				
//				Physics.Raycast(position,collisionSurfaceNormal,out info);
//				Vector3 exitPoint = position + collisionSurfaceNormal * info.distance;
//				
//	     		return collisionPoint + (exitPoint-collisionPoint)*(direction.magnitude-distance);
//			}
//	     }
	}
}