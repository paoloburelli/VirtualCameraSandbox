using UnityEngine;
using System.Collections;
using System.Threading;

[RequireComponent (typeof (Spawn))]
public class SandboxManager : MonoBehaviour
{

	public void Pause() {
		Time.timeScale = 0;
	}

	public void UnPause() {
		Time.timeScale = 1;
	}

	// Use this for initialization
	void Start ()
	{
		Transform t = GetComponent<Spawn>().SpawnInArea (GameObject.Find ("Forest").transform);
		t.GetComponent<RootMotionCharacterController>().MoveForward();

		t = GetComponent<Spawn>().SpawnInArea (GameObject.Find ("Forest").transform);
		t.GetComponent<RootMotionCharacterController>().MoveForward();

		t = GetComponent<Spawn>().SpawnInArea (GameObject.Find ("Forest").transform);
		t.GetComponent<RootMotionCharacterController>().MoveForward();

		t = GetComponent<Spawn>().SpawnInArea (GameObject.Find ("Forest").transform);
		t.GetComponent<RootMotionCharacterController>().MoveForward();

		t = GetComponent<Spawn>().SpawnInArea (GameObject.Find ("Forest").transform);
		t.GetComponent<RootMotionCharacterController>().MoveForward();

		t = GetComponent<Spawn>().SpawnInArea (GameObject.Find ("Forest").transform);
		t.GetComponent<RootMotionCharacterController>().MoveForward();

		t = GetComponent<Spawn>().SpawnInArea (GameObject.Find ("Forest").transform);
		t.GetComponent<RootMotionCharacterController>().MoveForward();

		t = GetComponent<Spawn>().SpawnInArea (GameObject.Find ("Forest").transform);
		t.GetComponent<RootMotionCharacterController>().MoveForward();

	}

	
	// Update is called once per frame
	void Update ()
	{
		if (Time.unscaledTime > 2)
			Pause ();

		if (Time.unscaledTime > 10)
			UnPause ();
	}
}

