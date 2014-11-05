using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	public Transform male;
	public Texture[] maleTextures;
	public Transform female;
	public Texture[] femaleTextures;

	private Transform instance;

	// Use this for initialization
	void Start () {
		instance = Instantiate(female, new Vector3 (0, 0, 0), Quaternion.identity) as Transform;
		instance.FindChild("Hips").renderer.material.SetTexture("_MainTex",femaleTextures[Mathf.FloorToInt(Random.value*femaleTextures.Length)]);
	}
	
	// Update is called once per frame
	void Update () {
		//instance.GetComponent<RootMotionCharacterControlFEMALE>().DefaultMovement = Vector3.forward;
		//instance.GetComponent<CharacterController>().SimpleMove(Vector3.forward);
	}
}
