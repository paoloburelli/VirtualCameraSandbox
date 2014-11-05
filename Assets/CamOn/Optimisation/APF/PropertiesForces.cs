using System;
using UnityEngine;
using CamOn.Properties;
using CamOn.Utils;
using System.Collections.Generic;

namespace CamOn.Optimisation.APF
{
	public static class PropertiesForces
	{
		public static Vector3 VisibilityPositionForce (List<SubjectEvaluator> subjects, Camera currentBest)
		{
			Vector3 centralPosition = Vector3.zero;
			float priority = 0;
			float strength = 0;
			foreach (SubjectEvaluator s in subjects)
				if (s != null && s.Active && !s.Ignore)
					foreach (Property p in s)
						if (p is Visibility && ((Visibility)p).DesiredVisibility > 0 && p.Satisfaction < 1 && !p.Target.AllOccluded) {
								centralPosition += p.Target.Position * p.Priority;
								priority += p.Priority;
								strength += Mathf.Abs(((Visibility)p).DesiredVisibility - p.Target.OnScreenFraction) * p.Priority;
						}
			
			if (priority > 0) {
				centralPosition = centralPosition / priority;
   				return (currentBest.transform.position-centralPosition).normalized * priority * strength/priority;
			}
			return Vector3.zero;
		}
		
		public static Vector3 PositionForce (this Property property, Camera currentBest)
		{
			switch (property.GetType ().Name) {
			case "Visibility":
				return PositionForce ((Visibility)property, currentBest) * property.Priority;
			case "ProjectionSize":
				return PositionForce ((ProjectionSize)property) * property.Priority;
			case "Angle":
			case "HorizontalAngle":
			case "VerticalAngle":
				return PositionForce ((Angle)property) * property.Priority;
			}
			return Vector3.zero;
		}
		
		private static Vector3 PositionForce (Visibility property, Camera currentBest)
		{
			int direction = property.DesiredVisibility > property.ActualVisibility ? 1 : -1;
			
			float h = 0;
			if (property.Target.LeftOccluded)
				h += direction;
			if (property.Target.RightOccluded)
				h -= direction;
			
			float v = 0;
			if (property.Target.BottomOccluded)
				v += direction;
			if (property.Target.TopOccluded)
				v -= direction;
			
			return (Vector3.up * v + currentBest.transform.right * h).normalized * (1 - property.Satisfaction) * property.Target.RelativeCameraPosition.magnitude;
		}
		
		private static Vector3 PositionForce (ProjectionSize property)
		{	
			float direction = 1;
			if (property.DesiredSize > property.ActualSize)
				direction = -1;
			
			if (property.ActualSize == 0)
				direction = 1;

			return direction * property.Target.RelativeCameraPosition.normalized * (1 - property.Satisfaction);
		}
		
		private static Vector3 PositionForce (Angle property)
		{
			Vector3 targetPosition = property.DesiredCameraDirection * property.Target.RelativeCameraPosition.magnitude;
			return (Vector3.RotateTowards (property.Target.RelativeCameraPosition, targetPosition, Mathf.PI, 1) - property.Target.RelativeCameraPosition) * (1 - property.Satisfaction);
		}
		
		public static Vector3 ViewpointForce (this Property property, Camera currentBest, Vector3 currentLookAtPoint)
		{
			switch (property.GetType ().Name) {
			case "Visibility":
				return ViewpointForce ((Visibility)property, currentBest, currentLookAtPoint) * property.Priority * property.Target.RelativeCameraPosition.magnitude / 10;
			case "PositionOnScreen":
				return ViewpointForce ((PositionOnScreen)property, currentBest, currentLookAtPoint) * property.Priority;
			}
			return Vector3.zero;
		}
		
		private static Vector3 ViewpointForce (Visibility property,  Camera currentBest, Vector3 currentLookAtPoint)
		{
						float direction = 1;
					if (property.DesiredVisibility < property.Target.OnScreenFraction)
						direction = -1;
					
					//Vector3 campos = property.Target.RelativeCameraPosition + property.Target.Position;
					Vector3 normalisedLookAtPoint = (currentLookAtPoint - currentBest.transform.position).normalized * property.Target.RelativeCameraPosition.magnitude + currentBest.transform.position;
					
					Vector3 vec = (property.Target.Position - normalisedLookAtPoint);
					if (vec.magnitude < 0.1f && direction == -1) {
						vec = UnityEngine.Random.onUnitSphere;
					}
					
					if (property.Satisfaction < 1 && !property.Target.AllOccluded)
						return direction * vec.normalized * Mathf.Abs (property.DesiredVisibility - property.Target.OnScreenFraction);
					else
						return Vector3.zero;
//			float v = property.Target.PositionOnScreen.y > Screen.height/2 ? -1 : 1;
//			float h = property.Target.PositionOnScreen.x > Screen.width/2 ? 1 : -1;
//
//			return (currentBest.transform.up * v + currentBest.transform.right * h).normalized * Mathf.Abs (property.DesiredVisibility - property.Target.OnScreenFraction);
		}


		private static Vector3 ViewpointForce (PositionOnScreen property, Camera currentBest, Vector3 currentLookAtPoint)
		{
			float v = property.DesiredScreenPosition.y > property.ActualScreenPosition.y ? 1 : -1;
			float h = property.DesiredScreenPosition.x > property.ActualScreenPosition.x ? -1 : 1;
			float magnitude = 0.5f*(currentBest.transform.position-currentLookAtPoint).magnitude / (currentBest.transform.position - property.Target.Position).magnitude;
			
			return magnitude*(currentBest.transform.up * v + currentBest.transform.right * h).normalized * (1 - property.Satisfaction);
//			return Vector3.zero;
		}		
	}
}

