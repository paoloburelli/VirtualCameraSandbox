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
	
	public Transform SpawnInArea(Transform area){
		int index = Mathf.FloorToInt (Random.value * GetComponent<Map>().GetSpwanPoints(area).Length);
		return SpawnAtSpawnPoint(area,index);
	}
	
	public Transform SpawnAtSpawnPoint(Transform area,int index){
		return SpawnAtSpawnPoint(area,index,Vector3.up);
	}

	public Transform SpawnAtSpawnPoint(Transform area,int index,Vector2 front,int combinationIndex = -1){
		Vector3 offset = Vector3.zero;
		if (occupiedLocations.Count (a => a.Key == area && a.Value == index) > 0) {
			offset = Random.onUnitSphere;
			offset.y = 0;
			offset.Normalize ();
		}
		
		occupiedLocations.Add (new KeyValuePair<Transform, int> (area, index));
		return SpawnAtPosition (GetComponent<Map>().GetSpwanPoints(area)[index]+offset/2,front,combinationIndex);
	}

	public Transform SpawnAtPosition(Vector3 position,Vector2 front,int combinationIndex = -1) {
		if (combinations.Count > 0) 
		{
			KeyValuePair<Transform,Texture> c;
			if (combinationIndex < 0 || combinationIndex >= combinations.Count) {
				int i = Mathf.FloorToInt (Random.value * combinations.Count);
				c = combinations [i];
				combinations.RemoveAt (i);
			} else
				c = combinations[combinationIndex];



			Transform t = Instantiate (c.Key, position, Quaternion.identity) as Transform;

			t.rotation = Quaternion.FromToRotation(t.forward,new Vector3(front.x,t.forward.y,front.y));

			t.FindChild("Hips").GetComponent<Renderer>().material.SetTexture("_MainTex",c.Value);
			return t;
		} else
			return null;
	}

	public Transform SpawnAtPosition(Vector3 position,int combinationIndex = -1) {
		return SpawnAtPosition(position,Vector2.up,combinationIndex);
	}

	// Update is called once per frame
	void Update () {

	}
}
