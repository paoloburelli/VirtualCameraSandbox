using UnityEngine;
using System.Collections;

public class ManualControl : MonoBehaviour {

	float rotationSpeed = 2;
	float zoomSpeed = 0.2f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<CameraOperator>().Shot.GetProperty<VantageAngle>(0) != null)
			GetComponent<CameraOperator>().Shot.GetProperty<VantageAngle>(0).DesiredHorizontalAngle += Input.GetAxis("Horizontal")*rotationSpeed;

		if (GetComponent<CameraOperator>().Shot.GetProperty<ProjectionSize>(0) != null)
			GetComponent<CameraOperator>().Shot.GetProperty<ProjectionSize>(0).DesiredSize *= 1+Input.GetAxis("Vertical")*zoomSpeed;
	}
}
