using UnityEngine;
using System.Collections;
using System;

public class Map : MonoBehaviour {

	public Transform[] Areas;
	private Vector3[][] SpawnPoints;

	public Vector3[] GetSpwanPoints(Transform area) {
		for (int i = 0; i<Areas.Length;i++)
			if (Areas[i] == area)
				return SpawnPoints[i];
		return new Vector3[0];
	}

	// Use this for initialization
	void Awake () {
		SpawnPoints = new Vector3[Areas.Length][];
		for (int i = 0; i<Areas.Length; i++) {
			Transform sp = Areas[i].FindChild ("Spawn Points");
			if (sp != null) {
				SpawnPoints[i] = new Vector3[sp.childCount];

				for (int j = 0;j<SpawnPoints[i].Length;j++) {
					SpawnPoints[i][j] = sp.GetChild(j).position;
				}
			} else {
				SpawnPoints[i] = new Vector3[0];
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
