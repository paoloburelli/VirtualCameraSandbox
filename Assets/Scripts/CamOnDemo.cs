using UnityEngine;
using System.Collections;
using System.Threading;

[RequireComponent (typeof (Spawn))]
[RequireComponent (typeof (Map))]
[RequireComponent (typeof (GUIText))]
public class CamOnDemo : MonoBehaviour
{
	public void Pause() {
		Time.timeScale = 0;
	}

	public void UnPause() {
		Time.timeScale = 1;
	}

	Transform a,b;
	string currentShotName;

	// Use this for initialization
	void Start ()
	{
		transform.position = Vector3.up;
		GetComponent<GUIText>().alignment = TextAlignment.Left;
		GetComponent<GUIText>().anchor = TextAnchor.UpperLeft;

		a = GetComponent<Spawn>().SpawnAtSpawnPoint (GetComponent<Map>().Areas[0],8,-Vector2.right);
		b = GetComponent<Spawn>().SpawnAtSpawnPoint (GetComponent<Map>().Areas[0],8,-Vector2.right);

		b.LookAt(a.position);
		a.LookAt(b.position);

		a.GetComponent<RootMotionCharacterController>().Talk();
		b.GetComponent<RootMotionCharacterController>().Talk();

		currentShotName = "TwoActors-OverTheShoulder";

		Camera.main.GetComponent<CameraOperator>().SelectShot(
			Resources.Load<Shot>(currentShotName),
			new Transform[]{
				a.FindChild("Hips"),
				b.FindChild("Hips")
			}
		);

		StartCoroutine(WalkAway(3));
	}

	IEnumerator WalkAway(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		a.GetComponent<RootMotionCharacterController>().TurnLeft();
		yield return new WaitForSeconds(0.5f);

		a.GetComponent<RootMotionCharacterController>().MoveForward();

		StartCoroutine(ChangeToA(0));
	}

	IEnumerator ChangeToA(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		if (Random.value < 0.5f)
			currentShotName = "OneActor-Follow";
		else
			currentShotName = "OneActor-LeftSide";

		Camera.main.GetComponent<CameraOperator>().SelectShot(Resources.Load<Shot>(currentShotName),new Transform[]{a.FindChild("Hips")});
		StartCoroutine(ChangeToB(10));
	}

	IEnumerator ChangeToB(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		if (Random.value < 0.5f)
			currentShotName = "OneActor-CloseUp";
		else
			currentShotName = "OneActor-LongShot";

		Camera.main.GetComponent<CameraOperator>().SelectShot(Resources.Load<Shot>(currentShotName),new Transform[]{b.FindChild("Hips")});
		StartCoroutine(ChangeToA(10));
	}

	// Update is called once per frame
	void Update ()
	{
		GetComponent<GUIText>().text = currentShotName;
	}
}

