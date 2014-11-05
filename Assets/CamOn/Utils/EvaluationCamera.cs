using System;
using UnityEngine;

namespace CamOn.Utils
{
	public static class EvaluationCamera
	{
		private static GameObject camObject = null;
		
		public static Camera camera {
			get {
				if (camObject == null) {
					camObject = new GameObject("[Evaluation Camera]");
					camObject.AddComponent<Camera> ();
					camObject.camera.enabled = false;
					camObject.camera.CopyFrom (Camera.main);
				}
				return camObject.camera;
			}
		}
	}
}

