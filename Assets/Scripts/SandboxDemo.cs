using UnityEngine;
using System.Collections;
using System.Threading;

[RequireComponent (typeof (Spawn))]
public class SandboxDemo : MonoBehaviour
{
	public void Pause() {
		Time.timeScale = 0;
	}

	public void UnPause() {
		Time.timeScale = 1;
	}

	Transform a,b;

	// Use this for initialization
	void Start ()
	{
		a = GetComponent<Spawn>().SpawnAtSpawnPoint (GameObject.Find ("House").transform,8,-Vector2.right);
		b = GetComponent<Spawn>().SpawnAtSpawnPoint (GameObject.Find ("House").transform,8,-Vector2.right);

		b.LookAt(a.position);
		a.LookAt(b.position);

		a.GetComponent<RootMotionCharacterController>().Talk();
		b.GetComponent<RootMotionCharacterController>().Talk();

		Camera.main.GetComponent<Operator>().SelectShot(
			Resources.Load<Shot>("TwoActors-OverTheShoulder"),
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
		b.GetComponent<RootMotionCharacterController>().TurnRight();
		yield return new WaitForSeconds(0.5f);

		a.GetComponent<RootMotionCharacterController>().MoveForward();
		b.GetComponent<RootMotionCharacterController>().MoveForward();

		StartCoroutine(ChangeToA(0));
	}

	IEnumerator ChangeToA(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		Camera.main.GetComponent<Operator>().SelectShot(Resources.Load<Shot>("OneActor-Follow"),new Transform[]{a.FindChild("Hips")});
		StartCoroutine(ChangeToB(10));
	}

	IEnumerator ChangeToB(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		Camera.main.GetComponent<Operator>().SelectShot(Resources.Load<Shot>("OneActor-Follow"),new Transform[]{b.FindChild("Hips")});
		StartCoroutine(ChangeToA(10));
	}

	// Update is called once per frame
	void Update ()
	{

	}
}

