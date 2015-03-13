using UnityEngine;
using System.Collections;
using System.Threading;
using System.IO;
using System.Collections.Generic;

public class wiced2015 : MonoBehaviour
{
	List<KeyValuePair<string,string>> sequence = new List<KeyValuePair<string,string>>();

	
	public void Pause() {
		Time.timeScale = 0;
	}
	
	public void UnPause() {
		Time.timeScale = 1;
	}

	public int Repetitions = 10;
	public float Duration = 10;
	int sequenceIndex = 0;
	TextWriter f;
	Actor a,b,c;
	GameObject sandbox;
	Spawn spawn;
	Map map;
	
	// Use this for initialization
	void Start ()
	{
		for (int i=0;i<Repetitions;i++){
			sequence.Add(new KeyValuePair<string,string>("Forest","All-1"));
			sequence.Add(new KeyValuePair<string,string>("House","All-1"));
			sequence.Add(new KeyValuePair<string,string>("House","All-3"));
			sequence.Add(new KeyValuePair<string,string>("Rocks","All-3"));
		}

		GetComponent<GUIText>().text = "Loading "+sequence[sequenceIndex].Key+" Scene...";
		Application.LoadLevel(sequence[sequenceIndex].Key);
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

			a = Actor.Create(spawn.SpawnAtSpawnPoint (map.Areas[0],Random.Range(0,6),Vector2.right * Random.Range(-1f,1f)),PrimitiveType.Capsule,Vector3.up,new Vector3(.5f,.9f,.5f));
			b = Actor.Create(spawn.SpawnAtSpawnPoint (map.Areas[0],Random.Range(0,6),Vector2.right * Random.Range(-1f,1f)),PrimitiveType.Capsule,Vector3.up,new Vector3(.5f,.9f,.5f));
			c = Actor.Create(spawn.SpawnAtSpawnPoint (map.Areas[0],Random.Range(0,6),Vector2.right * Random.Range(-1f,1f)),PrimitiveType.Capsule,Vector3.up,new Vector3(.5f,.9f,.5f));
			
			
			a.transform.GetComponent<RootMotionCharacterController>().MoveForward();
			b.transform.GetComponent<RootMotionCharacterController>().MoveForward();
			c.transform.GetComponent<RootMotionCharacterController>().MoveForward();
			
			CameraOperator.OnMainCamera.SelectShot(Resources.Load<Shot>(sequence[sequenceIndex].Value),
			                                       CameraOperator.Transition.Cut,
			                                       new Actor[]{a,b,c});

			Directory.CreateDirectory(Application.dataPath+"/Logs/");
			f =  File.CreateText(Application.dataPath+"/Logs/"+(sequenceIndex/4)+"."+sequence[sequenceIndex].Key+"-"+sequence[sequenceIndex].Value+".csv");
			StartCoroutine(Next (Duration));

			string header = "CameraX,CameraY,CameraZ,";

			foreach (Property p in CameraOperator.OnMainCamera.Shot.Properties)
				header += p.PropertyType + "(" + p.MainSubjectIndex + "), ";

			for (int i = 0; i<CameraOperator.OnMainCamera.Actors.Length;i++)
				header += "Visibility(" +i+ "), ";

			header += "Quality";

			f.WriteLine(header);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (sandbox != null)
			sandbox.GetComponent<GUIText>().text = sequence[sequenceIndex].Value;

		if (f!= null) {

			CameraOperator.OnMainCamera.Shot.GetQuality(CameraOperator.OnMainCamera.Actors,	Camera.main);
			string line=Camera.main.transform.position.x+", "+Camera.main.transform.position.y+", "+Camera.main.transform.position.x+", ";

			foreach (Property p in CameraOperator.OnMainCamera.Shot.Properties)
				line += p.Satisfaction + ", ";
			
			foreach (Actor a in CameraOperator.OnMainCamera.Actors)
				line += a.Visibility + ", ";
			
			line += CameraOperator.OnMainCamera.Shot.GetQuality(CameraOperator.OnMainCamera.Actors);

			f.WriteLine(line);
		}
	}
	
	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	IEnumerator Next(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		f.Close();
		f = null;
		sequenceIndex++;

		if (sequenceIndex < sequence.Count)
			Application.LoadLevel(sequence[sequenceIndex].Key);
		else
			Application.Quit();
	}

	void OnApplicationQuit() {
		if (f!= null)
			f.Close();
	}
}

