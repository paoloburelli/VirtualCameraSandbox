using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent (typeof (Map))]
public class Spawn : MonoBehaviour {

	public Transform male;
	public Texture[] maleTextures;
	public Transform female;
	public Texture[] femaleTextures;
	
	List<KeyValuePair<Transform,Texture>> combinations;

	List<KeyValuePair<Transform,int>> occupiedLocations;

	// Use this for initialization
	void Awake () {
		combinations = new List<KeyValuePair<Transform, Texture>> ();
		occupiedLocations = new List<KeyValuePair<Transform, int>> ();

		foreach (Texture t in maleTextures)
						combinations.Add (new KeyValuePair<Transform, Texture> (male, t));

		foreach (Texture t in femaleTextures)
			combinations.Add (new KeyValuePair<Transform, Texture> (female, t));
	}
	
	public void SpawnInArea(Transform area){
		int index = Mathf.FloorToInt (Random.value * GetComponent<Map>().GetSpwanPoints(area).Length);
		SpawnAtSpawnPoint(area,index);
	}
	
	public void SpawnAtSpawnPoint(Transform area,int index){
		Vector3 offset = Vector3.zero;
		if (occupiedLocations.Count (a => a.Key == area && a.Value == index) > 0) {
						offset = Random.onUnitSphere;
						offset.y = 0;
						offset.Normalize ();
		}
		
		SpawnAtPosition (GetComponent<Map>().GetSpwanPoints(area)[index]+offset/2);
		occupiedLocations.Add (new KeyValuePair<Transform, int> (area, index));
	}

	public Transform SpawnAtPosition(Vector3 position) {
		if (combinations.Count > 0) 
		{
			int i = Mathf.FloorToInt (Random.value * combinations.Count);
			KeyValuePair<Transform,Texture> c = combinations [i];
			combinations.RemoveAt (i);
			
			Transform t = Instantiate (c.Key, position, Quaternion.identity) as Transform;
			t.FindChild("Hips").renderer.material.SetTexture("_MainTex",c.Value);
			return t;
		} else
			return null;
	}

	// Update is called once per frame
	void Update () {

	}
}
