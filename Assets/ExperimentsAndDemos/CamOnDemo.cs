using UnityEngine;
using System.Collections;
using System.Threading;

public class CamOnDemo : MonoBehaviour
{
	public string Scene="Forest";

	public void Pause() {
		Time.timeScale = 0;
	}

	public void UnPause() {
		Time.timeScale = 1;
	}
	
	string currentShotName;
	Actor a,b;
	GameObject sandbox;
	Spawn spawn;
	Map map;

	// Use this for initialization
	void Start ()
	{
		GetComponent<GUIText>().text = "Loading "+Scene+" Scene...";
		Application.LoadLevel(Scene);
	}

	void OnLevelWasLoaded(int level) {
		Destroy(GetComponent<GUIText>());
		sandbox = GameObject.Find("Sandbox");
		spawn = sandbox.GetComponent<Spawn>();
		map = sandbox.GetComponent<Map>();

		sandbox.transform.position = Vector3.up;
		sandbox.GetComponent<GUIText>().alignment = TextAlignment.Left;
		sandbox.GetComponent<GUIText>().anchor = TextAnchor.UpperLeft;
		
		a = Actor.Create(spawn.SpawnAtSpawnPoint (map.Areas[0],8,-Vector2.right),PrimitiveType.Capsule,Vector3.up,new Vector3(.5f,.9f,.5f));
		b = Actor.Create(spawn.SpawnAtSpawnPoint (map.Areas[0],8,-Vector2.right),PrimitiveType.Capsule,Vector3.up,new Vector3(.5f,.9f,.5f));
		
		a.transform.LookAt(b.transform.position);
		b.transform.LookAt(a.transform.position);
		
		a.transform.GetComponent<RootMotionCharacterController>().Talk();
		b.transform.GetComponent<RootMotionCharacterController>().Talk();
		
		currentShotName = "TwoActors-OverTheShoulder";
		
		CameraOperator.OnMainCamera.SelectShot(Resources.Load<Shot>(currentShotName),
		                                       CameraOperator.Transition.Cut,
		                                       new Actor[]{a,b});
		
		StartCoroutine(WalkAway(3));
	}

	IEnumerator WalkAway(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		a.transform.GetComponent<RootMotionCharacterController>().TurnLeft();
		yield return new WaitForSeconds(0.5f);
		a.transform.GetComponent<RootMotionCharacterController>().MoveForward();

		StartCoroutine(ChangeToA(0));
	}

	IEnumerator ChangeToA(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		if (Random.value < 0.5f)
			currentShotName = "OneActor-Follow";
		else
			currentShotName = "OneActor-LeftSide";


		CameraOperator.OnMainCamera.SelectShot(Resources.Load<Shot>(currentShotName),
		                                       CameraOperator.Transition.Smooth,
		                                       new Actor[]{a});
		StartCoroutine(ChangeToB(10));
	}

	IEnumerator ChangeToB(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		if (Random.value < 0.5f)
			currentShotName = "OneActor-CloseUp";
		else
			currentShotName = "OneActor-LongShot";


		CameraOperator.OnMainCamera.SelectShot(Resources.Load<Shot>(currentShotName),
		                                       CameraOperator.Transition.Cut,
		                                       new Actor[]{b});
		StartCoroutine(ChangeToA(10));
	}

	// Update is called once per frame
	void Update ()
	{
		if (sandbox != null)
			sandbox.GetComponent<GUIText>().text = currentShotName;
	}

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}
}

