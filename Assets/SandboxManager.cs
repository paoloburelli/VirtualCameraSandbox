using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Spawn))]
public class SandboxManager : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		GetComponent<Spawn>().SpawnInArea (Map.Area.House);
		GetComponent<Spawn>().SpawnInArea (Map.Area.House);
	}

	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

