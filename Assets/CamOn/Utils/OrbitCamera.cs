using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour {
	
	public Transform Target;
	public float Distance = 10.0f;
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	public float zoomSpeed = 250.0f;

	public float yMinLimit = -20;
	public float yMaxLimit = 80;
	public float distanceMaxLimit = 20;
	public float distanceMinLimit = 5;

	private float x = 0.0f;
	private float y = 0.0f;
	
	public bool OnClick = false;
	
	// Use this for initialization
	public void Start ()
	{
		//Vector3 angles = transform.eulerAngles;
		x = 0;
		y = 30;
	
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
		
		x += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
		y += Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;
		Distance -= Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed * 0.02f;
		
		y = ClampAngle (y, yMinLimit, yMaxLimit);
		Distance = Mathf.Clamp (Distance, distanceMinLimit, distanceMaxLimit);
		
		Quaternion rotation = Quaternion.Euler (y, x, 0);
		Vector3 position = rotation * new Vector3 (0, 0, -Distance) + Target.position;
		
		transform.rotation = rotation;
		transform.position = position;
	}
	
	void Update ()
	{
		//if ((Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1) || 
		if ((Input.GetMouseButtonDown (1)) && Input.mousePosition.y < Screen.height || !OnClick)
			Screen.lockCursor = true;
		
		//if (!Screen.fullScreen && Input.GetKeyDown ("escape"))
		if (Input.GetMouseButtonUp (1))
			Screen.lockCursor = false;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		if (Target && Screen.lockCursor) {
			x += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
			y += Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;
			
			
		}
		Distance -= Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed * 0.02f;
			
			y = ClampAngle (y, yMinLimit, yMaxLimit);
			Distance = Mathf.Clamp (Distance, distanceMinLimit, distanceMaxLimit);
			
			Quaternion rotation = Quaternion.Euler (y, x, 0);
			Vector3 position = rotation * new Vector3 (0, 0, -Distance) + Target.position;
			
			transform.rotation = rotation;
			transform.position = position;
		
	}
	
	
	public float ClampAngle (float angle, float min, float max) {
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}
}