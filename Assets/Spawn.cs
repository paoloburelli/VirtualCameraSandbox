using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawn : MonoBehaviour {
	
	public Transform male;
	public Texture[] maleTextures;
	public Transform female;
	public Texture[] femaleTextures;

	Transform instance;

	List<KeyValuePair<Transform,Texture>> combinations;

	// Use this for initialization
	void Start () {
		combinations = new List<KeyValuePair<Transform, Texture>> ();

		foreach (Texture t in maleTextures)
						combinations.Add (new KeyValuePair<Transform, Texture> (male, t));

		foreach (Texture t in femaleTextures)
			combinations.Add (new KeyValuePair<Transform, Texture> (female, t));

		instance = spawnNewCharacter (Vector3.zero);
		instance.GetComponent<RootMotionCharacterController> ().MoveForward();
	}

	public Transform spawnNewCharacter(Vector3 position) {
		if (combinations.Count > 0) 
		{
			int i = Mathf.FloorToInt (Random.value * combinations.Count);
			KeyValuePair<Transform,Texture> c = combinations [i];
			combinations.RemoveAt (i);

			return(Instantiate (c.Key, position, Quaternion.identity) as Transform);
		} else
			return null;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > 5)
			instance.GetComponent<RootMotionCharacterController> ().Laugh ();
	}
}
