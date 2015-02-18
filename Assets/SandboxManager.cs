using UnityEngine;
using System.Collections;
using System.Threading;

[RequireComponent (typeof (Spawn))]
public class SandboxManager : MonoBehaviour
{
	Subject[] subjects;
	Shot shot;


	public void Pause() {
		Time.timeScale = 0;
	}

	public void UnPause() {
		Time.timeScale = 1;
	}

	// Use this for initialization
	void Start ()
	{
		Transform t = GetComponent<Spawn>().SpawnAtSpawnPoint (GameObject.Find ("Rocks").transform,0,-Vector2.right);
		t.GetComponent<RootMotionCharacterController>().Talk();

		Camera.main.GetComponent<Operator>().Shot = Resources.Load<Shot>("LongShot");
		Camera.main.GetComponent<Operator>().SetSubjectTransform(0,t);
		Camera.main.GetComponent<Operator>().SetSubjectCenter(0,Vector3.up);
//
//		shot = new Shot();
//		shot.NumberOfSubjects = 1;
//		shot.Properties.Add (new ProjectionSize(0,1,1));
//		shot.Properties.Add(new VantageAngle(0,0,0,1));
//		shot.SubjectCenters.Add (new Vector3(0,1.6f,0));
//		shot.SubjectScales.Add (new Vector3(.2f,.2f,.2f));
//		shot.SubjectBounds.Add (PrimitiveType.Capsule);
//
//		subjects  = new Subject[shot.NumberOfSubjects];
//		for (int i = 0; i<shot.NumberOfSubjects; i++)
//			subjects[i] = new Subject(t,shot.SubjectCenters[i],shot.SubjectScales[i],shot.SubjectBounds[i]); 
//
//		Camera.main.transform.position = t.position + t.forward;
//
//		Camera.main.transform.LookAt(t.position + Vector3.up*1.6f);
//		Camera.main.transform.Translate(Vector3.up);
//
//		shot.UpdateSubjects(subjects,Camera.main);
//		shot.Evaluate ();
//
//		foreach (Property p in shot.Properties) {
//			Debug.Log(p.Type + ": " + p.Satisfaction);
//		}
//		Debug.Log ("Visibility: " + shot.Visibility);	
	}

	
	// Update is called once per frame
	void Update ()
	{

	}


	void OnDrawGizmos() {
		if (subjects != null)
			foreach (Subject s in subjects)
				if (s != null) 
					s.DrawGizmos ();
	}
}

