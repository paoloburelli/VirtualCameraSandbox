using UnityEngine;
using System.Collections;
using System;

public class Map : MonoBehaviour {

	public Transform[] Areas;
	public Vector3[][] SpawnPoints;

	// Use this for initialization
	void Awake () {
		SpawnPoints = new Vector3[Areas.Length][];
		for (int i = 0; i<Areas.Length; i++) {
			Transform sp = Areas[i].FindChild ("Spawn Points");
			SpawnPoints[i] = new Vector3[sp.childCount];
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
