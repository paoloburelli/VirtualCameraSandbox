using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CamOn;
using CamOn.Properties;
using CamOn.Utils;

namespace CamOn
{
	public class SubjectBackend : MonoBehaviour
	{
		const int LAYER_MASK = ~6; //do not test transparent and ignore raycast;
		
		public const string BOUNDS_NAME = "<CamOn Camera Bounds>";
		
		public const string IGNORE_TAG = "IGNORE";
		public bool Ignore {
			get {
				return tag == IGNORE_TAG;
			}
			set {
				tag = value ? IGNORE_TAG : "Untagged";
			}
		}

		public Quaternion Rotation {
			get { return CameraBoundsRenderer.transform.rotation; 
			}
		}

		private List<Property> properties = new List<Property> ();
		public IEnumerator GetEnumerator ()
		{
			return properties.GetEnumerator ();
		}
		
		public void AddProperty (Property property)
		{
			property.Target = this;
			properties.Add (property);
		}

		public void RemoveProperty<T> () where T : Property
		{
			properties.RemoveAll (item => item is T);
		}

		public void ClearAllProperties ()
		{
			properties.Clear ();
		}
		
		public T GetProperty<T> () where T : Property
		{
			foreach (Property p in properties)
				if (p is T)
					return (T)p;
			return null;
		}

		public string PropertiesDescription {
			get {
				char[] comma = { ',' };
				string output = "";
				foreach (Property p in properties)
					output += p.GetType ().Name + ",";
				return output.TrimEnd (comma);
			}
		}
		
		float onScreenFraction;
		public float OnScreenFraction {
			get { return onScreenFraction; }
		}		
		
		float projectionSize;
		public float ProjectionSize {
			get { return projectionSize; }
		}

		float onScreenSize;
		public float ProjectionSizeOnScreen {
			get { return onScreenSize; }
		}

		Vector2 projectionPosition;
		public Vector2 PositionOnScreen {
			get { return projectionPosition; }
		}

		public Vector3 Position {
			get { return CameraBoundsRenderer == null ? Vector3.zero : CameraBoundsRenderer.gameObject.transform.position; }
		}

		public Vector3 Forward {
			get { return CameraBoundsRenderer == null ? Vector3.forward : CameraBoundsRenderer.gameObject.transform.parent.forward; }
		}

		public Vector2 Orientation {
			get { return CameraBoundsRenderer == null ? Vector2.zero : CameraBoundsRenderer.gameObject.transform.parent.GetSphericalRotation (); }
		}

		public Vector3 RelativeCameraPosition {
			get { return relativeCameraPosition; }
		}

		private Vector3 relativeCameraPosition;
		public float CameraAngle {
			get { return Vector3.Angle (Forward, relativeCameraPosition.normalized); }
		}

		private Vector2 cameraSphericalAngle = Vector2.zero;
		public Vector2 CameraSphericalAngles {
			get {
				float horizontalCameraAngle = Mathf.Atan2 (relativeCameraPosition.normalized.x, relativeCameraPosition.normalized.z) * 180 / Mathf.PI - Orientation.x;
				if (horizontalCameraAngle > 180)
					horizontalCameraAngle -= 360;
				if (horizontalCameraAngle < -180)
					horizontalCameraAngle += 360;
				cameraSphericalAngle.x = horizontalCameraAngle;
				cameraSphericalAngle.y = Mathf.Asin (relativeCameraPosition.normalized.y) * 180 / Mathf.PI - Orientation.y;
				return cameraSphericalAngle;
			}
		}
		
		private GameObject[] occluders= new GameObject[5];
		public string Occluders {
			get {
				string oc = "";
				foreach (GameObject go in occluders)
					oc += go != null ? go.name + ",": "none,";
				return oc.TrimEnd (',');
			}
		}
		
		public float Visibility {
			get {
				float occlusion = 0;
				
				Vector3[] vertices = { Top, Bottom, Left, Right, Center };
				if (currentCamera != null && CameraBoundsRenderer != null && onScreenFraction == 0){
					vertices[0] = CameraBoundsRenderer.bounds.center + currentCamera.transform.up*CameraBoundsRenderer.bounds.extents.magnitude/2;
					vertices[1] = CameraBoundsRenderer.bounds.center - currentCamera.transform.up * CameraBoundsRenderer.bounds.extents.magnitude / 2;
					vertices[2] = CameraBoundsRenderer.bounds.center - currentCamera.transform.right * CameraBoundsRenderer.bounds.extents.magnitude / 2;
					vertices[3] = CameraBoundsRenderer.bounds.center + currentCamera.transform.right * CameraBoundsRenderer.bounds.extents.magnitude / 2;
					vertices[4] = CameraBoundsRenderer.bounds.center;
				}
				
				topOccluded = bottomOccluded = leftOccluded = rightOccluded = centerOccluded = false;
				
				float occlusionResolution = 1.0f / vertices.Length;
				
				RaycastHit hitInfo = new RaycastHit ();

				for (int i=0;i<vertices.Length;i++) {
					if (currentCamera != null) {
//						Vector3 direction = (vertices[i] - currentCamera.transform.position).normalized;
//						Vector3 origin = currentCamera.transform.position + direction * currentCamera.nearClipPlane/Vector3.Dot(direction.normalized,Forward);
//						float distance = (vertices[i] - currentCamera.transform.position).magnitude - currentCamera.nearClipPlane * 1.2f * currentCamera.nearClipPlane / Vector3.Dot (direction, Forward);

						Vector3 direction = (currentCamera.transform.position - vertices[i]).normalized;
						Vector3 origin = vertices[i];
						float distance = (vertices[i] - currentCamera.transform.position).magnitude-currentCamera.nearClipPlane* currentCamera.nearClipPlane / Vector3.Dot (-direction, Forward);
						
						occluders[i] = null;
						if ( distance > 0 && Physics.Raycast (origin, direction, out hitInfo, distance,LAYER_MASK))
							if (hitInfo.collider.gameObject != gameObject && hitInfo.collider.gameObject.GetComponentInChildren<SubjectEvaluator> () != this) {
								
								occluders[i] = hitInfo.collider.gameObject;
								occlusion += occlusionResolution;
								switch (i) {
								case 0:
									topOccluded = true;
									break;
								case 1:
									bottomOccluded = true;
									break;
								case 2:
									leftOccluded = true;
									break;
								case 3:
									rightOccluded = true;
									break;
								case 4:
									centerOccluded = true;
									break;
								}
							}
					}
				}
				
				float maxVisibleFraction = projectionSize < 1 ? 1 : 1 / projectionSize;
				return onScreenFraction / maxVisibleFraction * (1 - occlusion);
			}
		}

		Vector3 visibleTop;
		float screenTop;
		public Vector3 Top {
			get { return visibleTop; }
		}
		public float ScreenTop {
			get { return screenTop; }
		}
		protected bool topOccluded = false;
		public bool TopOccluded {get { return topOccluded;}}

		Vector3 visibleBottom;
		float screenBottom;
		public Vector3 Bottom {
			get { return visibleBottom; }
		}
		public float ScreenBottom {
			get { return screenBottom; }
		}
		protected bool bottomOccluded = false;
		public bool BottomOccluded {
			get { return bottomOccluded; }
		}

		Vector3 visibleLeft;
		float screenLeft;
		public Vector3 Left {
			get { return visibleLeft; }
		}
		public float ScreenLeft {
			get { return screenLeft; }
		}
		protected bool leftOccluded = false;
		public bool LeftOccluded {
			get { return leftOccluded; }
		}

		Vector3 visibleRight;
		float screenRight;
		public Vector3 Right {
			get { return visibleRight; }
		}
		public float ScreenRight {
			get { return screenRight; }
		}
		protected bool rightOccluded = false;
		public bool RightOccluded {
			get { return rightOccluded; }
		}

		public Vector3 Center {
			get { return (visibleTop + visibleBottom + visibleLeft + visibleRight) / 4; }
		}
		protected bool centerOccluded = false;
		public bool CenterOccluded {
			get { return centerOccluded; }
		}
		
		public bool AllOccluded {
			get { return TopOccluded && BottomOccluded && RightOccluded && LeftOccluded && CenterOccluded; }
		}
		
		public bool AllUnoccluded {
			get { return !TopOccluded && !BottomOccluded && !RightOccluded && !LeftOccluded && !CenterOccluded; }
		}
		
		public float Priority {
			get {
				return properties.Sum (n => n.Priority);
			}
		}
				
		public float EvaluateProperties (Camera cam)
		{
			ManualUpdate (cam);
			float satisfaction = 0;
			foreach (Property p in properties) {
				satisfaction += p.Satisfaction * p.Priority;
			}
			satisfaction = satisfaction / Priority;
			
			if (properties.Count == 0)
				satisfaction = 1;
			
			return satisfaction;
		}
				
		private Mesh cameraBoundsMesh = null;
		public Mesh CameraBoundsMesh {
			get {
				if (cameraBoundsMesh == null)
					createCameraBounds (cameraBoundsType);
				return cameraBoundsMesh;
			}
		}
		private Renderer cameraBoundsRenderer = null;
		public Renderer CameraBoundsRenderer {
			get {
				if (cameraBoundsRenderer == null)
					createCameraBounds (cameraBoundsType);
				return cameraBoundsRenderer;
			}
		}
		
		private Vector3 cameraBoundsCenter = Vector3.zero;
		public Vector3 CameraBoundsCenter {
			get { return cameraBoundsCenter; }
			set {cameraBoundsCenter = value;CameraBoundsRenderer.transform.localPosition = cameraBoundsCenter;}
		}
			
		private Vector3 cameraBoundsScale = Vector3.one;
		public Vector3 CameraBoundsScale {
			get { return cameraBoundsScale; }
			set {cameraBoundsScale = value;CameraBoundsRenderer.transform.localScale = cameraBoundsScale;}
		}
			
		private Quaternion cameraBoundsRotation = Quaternion.identity;
		public Quaternion CameraBoundsRotation {
			get { return cameraBoundsRotation; }
			set {cameraBoundsRotation = value;CameraBoundsRenderer.transform.localRotation = cameraBoundsRotation;}
		}
		
		private PrimitiveType cameraBoundsType = PrimitiveType.Capsule;
		public PrimitiveType CameraBoundType {
			get { return cameraBoundsType; }
			set {
				if (CameraBoundsRenderer == null || value != cameraBoundsType) {
					cameraBoundsType = value;
					createCameraBounds (value);
				}
			}
		}
			
		private void resetCameraBounds ()
		{
			cameraBoundsRenderer = null;
			cameraBoundsMesh = null;
			while (gameObject.transform.Find (BOUNDS_NAME) != null)
				DestroyImmediate (gameObject.transform.Find (BOUNDS_NAME).gameObject);
		}
		
		private void createCameraBounds (PrimitiveType type)
		{
			resetCameraBounds ();
			if (Debug.isDebugBuild)
				Debug.Log ("New Bound Created");
			GameObject envelope = GameObject.CreatePrimitive (type);
			DestroyImmediate (envelope.collider);
			envelope.renderer.sharedMaterial = new Material (Shader.Find ("Transparent/Diffuse"));
			envelope.renderer.sharedMaterial.color = new Color (1, 0, 1, 0.4f);
			envelope.transform.parent = gameObject.transform;
			envelope.transform.localPosition = cameraBoundsCenter;
			envelope.transform.localRotation = cameraBoundsRotation;
			envelope.transform.localScale = cameraBoundsScale;
			envelope.name = BOUNDS_NAME;
			cameraBoundsRenderer = envelope.GetComponent<MeshRenderer> ();
			cameraBoundsMesh = envelope.GetComponent<MeshFilter> ().sharedMesh;
			envelope.SetActive(gameObject.activeSelf);
		}
		
		public void OnDestroy ()
		{
			resetCameraBounds ();
		}


		public bool HasVisibility {
			get {
				return !noVisibility;
			}
		}

		public bool HasProjectionSize {
			get {
				return !noProjectionSize;
			}
		}

		bool noVisibility;
		bool noProjectionSize;
		private Camera currentCamera;
		protected void ManualUpdate (Camera camera)
		{
			//Quaternion oldCamRotation = camera.transform.rotation;
			noProjectionSize = GetProperty<ProjectionSize> () == null;
			noVisibility = (GetProperty<Visibility> () == null && GetProperty<PositionOnScreen> () == null);
			if (noVisibility)
				camera.transform.LookAt (SubjectEvaluator.AllSubjectsCenter);
			
			if (!Ignore) {

				currentCamera = camera;

				if (cameraBoundsRenderer.bounds.Contains(camera.transform.position)){
					onScreenFraction = 0;
					projectionSize = float.PositiveInfinity;
					projectionPosition = new Vector2 (0,0);
				} else {

					screenTop = 0;
					screenBottom = 1;
					screenLeft = 1;
					screenRight = 0;
					int onScreen = 0;

					//string output = "";
					foreach (Vector3 v in CameraBoundsMesh.vertices) {
						Vector3 tv = CameraBoundsRenderer.gameObject.transform.TransformPoint (v);
						Vector3 sv = camera.WorldToViewportPoint (tv);
						//output += sv+ ", ";
						if (sv.z > 0 && sv.y < 1 && sv.y > 0 && sv.x > 0 && sv.x < 1) {
							onScreen++;
							if (sv.y > screenTop) {
								screenTop = sv.y;
								visibleTop = tv;
							}
							if (sv.y < screenBottom) {
								screenBottom = sv.y;
								visibleBottom = tv;
							}
							if (sv.x > screenRight) {
								screenRight = sv.x;
								visibleRight = tv;
							}
							if (sv.x < screenLeft) {
								screenLeft = sv.x;
								visibleLeft = tv;
							}
						}
					}
					//Debug.Log(output);
					
					screenTop = 1 - screenTop;
					screenBottom = 1 - screenBottom;
					onScreenFraction = (float)onScreen / CameraBoundsMesh.vertices.Length;
					onScreenSize = (screenBottom - screenTop) * (screenRight - screenLeft);
							
					
					//This fixes the projection size estimantion at the screen borders
					float projectionEstimantionFactor = onScreenFraction < 0.15f ? 0.15f : onScreenFraction;
					if (onScreenFraction == 0)
						projectionEstimantionFactor = 0;
					
					//This way the projection size consider the longest side and not the full area
					float verticalRatio = (screenBottom - screenTop) / projectionEstimantionFactor;
					float horizontalRatio = (screenRight - screenLeft) / projectionEstimantionFactor;
					projectionSize = verticalRatio > horizontalRatio ? verticalRatio : horizontalRatio;
					//Debug.Log(onScreen + "+" + projectionSize);
					projectionSize = projectionSize > 0 ? projectionSize : 0;
					
					relativeCameraPosition = camera.transform.position - Position;
					
					Vector3 centerPos = camera.WorldToViewportPoint (Position);
					projectionPosition = new Vector2 (centerPos.x, 1 - centerPos.y);
				
				}
			}

//			if (noVisibility)
//				camera.transform.rotation = oldCamRotation;
		}
	}
}
