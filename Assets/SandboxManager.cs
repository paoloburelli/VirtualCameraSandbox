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
		Transform t = GetComponent<Spawn>().SpawnAtSpawnPoint (GameObject.Find ("Forest").transform,0,-Vector2.right);
		t.GetComponent<RootMotionCharacterController>().Talk();

	}

	
	// Update is called once per frame
	void Update ()
	{
//		if (Time.unscaledTime > 2)
//			Pause ();
//
//		if (Time.unscaledTime > 10)
//			UnPause ();
	}
}

