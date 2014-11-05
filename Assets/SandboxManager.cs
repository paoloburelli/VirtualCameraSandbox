using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Spawn))]
public class SandboxManager : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		GetComponent<Spawn>().SpawnInArea (GameObject.Find ("House").transform);
		GetComponent<Spawn>().SpawnInArea (GameObject.Find ("House").transform);
	}

	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

