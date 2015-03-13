using UnityEngine;
using System.Collections;

public class wiced2015screenShots : MonoBehaviour {

	public string Scene="Forest";
	Actor a,b;
	GameObject sandbox;
	Spawn spawn;
	Map map;

	// Use this for initialization
	void Start () {
		GetComponent<GUIText>().text = "Loading "+Scene+" Scene...";
		Application.LoadLevel(Scene);
	}
	
	void OnLevelWasLoaded(int level) {
		if (enabled) {
			Destroy(GetComponent<GUIText>());
			sandbox = GameObject.Find("Sandbox");
			spawn = sandbox.GetComponent<Spawn>();
			map = sandbox.GetComponent<Map>();
			
			sandbox.transform.position = Vector3.up;
			sandbox.GetComponent<GUIText>().alignment = TextAlignment.Left;
			sandbox.GetComponent<GUIText>().anchor = TextAnchor.UpperLeft;
			
			a = Actor.Create(spawn.SpawnAtSpawnPoint (map.Areas[0],3,-Vector2.right,4),PrimitiveType.Capsule,Vector3.up,new Vector3(.5f,.9f,.5f));
			b = Actor.Create(spawn.SpawnAtSpawnPoint (map.Areas[0],5,-Vector2.right,2),PrimitiveType.Capsule,Vector3.up,new Vector3(.5f,.9f,.5f));
			
			a.transform.LookAt(b.transform.position);
			b.transform.LookAt(a.transform.position);
			
			a.transform.GetComponent<RootMotionCharacterController>().MoveForward();
			b.transform.GetComponent<RootMotionCharacterController>().Talk();

			CameraOperator.OnMainCamera.SelectShot(Resources.Load<Shot>("TwoActors-OverTheShoulder"),
			                                       CameraOperator.Transition.Cut,
			                                       new Actor[]{a,b});
			
			StartCoroutine(Record());
		}
	}

	IEnumerator Record() {
		yield return new WaitForSeconds(1f);
		for (int i=0;i<10;i++) {
			Application.CaptureScreenshot(Application.dataPath+"/Screenshot-"+i+".png");
			yield return new WaitForSeconds(0.5f);
		}
		Application.Quit();
	}

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}
}
