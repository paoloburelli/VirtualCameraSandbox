		
using UnityEngine;
using System.Collections;
using System.Threading;


[AddComponentMenu("Mixamo/Demo/Root Motion Character")]
public class RootMotionCharacterController : MonoBehaviour
{
	public float turningSpeed = 90f;
	public RootMotionComputer computer;
	public CharacterController character;

	public void MoveForward(float movementSpeed) {
		Stop ();
		startMovingForward = true;
		this.MovementSpeed = movementSpeed;
	}

	public void MoveForward() {
		MoveForward(1);
	}
	public void TurnLeft() {
		Stop ();
		turningLeft = true;
	}
	public void TurnRight() {
		Stop ();
		turningRight = true;
	}

	public void Talk() {
		Stop ();
		talk = true;
	}

	public void Laugh() {
		Stop ();
		laugh = true;
	}

	public void Stop() {
		movingForward = false;
		turningLeft = false;
		turningRight = false;
		talk = false;
		laugh = false;
	}

	private bool movingForward = false;
	private bool startMovingForward = false;
	private bool turningRight = false;
	private bool turningLeft = false;
	private bool talk = false;
	private bool laugh = false;

	private Collider area;
	private float MovementSpeed;

	Vector3 pos,forw;
	
	void Start()
	{
		// validate component references
		if (computer == null) computer = GetComponent(typeof(RootMotionComputer)) as RootMotionComputer;
		if (character == null) character = GetComponent(typeof(CharacterController)) as CharacterController;
		
		// tell the computer to just output values but not apply motion
		computer.applyMotion = false;
		// tell the computer that this script will manage its execution
		computer.isManagedExternally = true;
		// since we are using a character controller, we only want the z translation output
		computer.computationMode = RootMotionComputationMode.ZTranslation;
		// initialize the computer
		computer.Initialize();
		
		// set up properties for the animations
		GetComponent<Animation>()["weightshiftidle"].layer = 0; GetComponent<Animation>()["weightshiftidle"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>()["walk"].layer = 1; GetComponent<Animation>()["walk"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>()["run"].layer = 1; GetComponent<Animation>()["run"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>()["boredidle"].layer = 3; GetComponent<Animation>()["boredidle"].wrapMode = WrapMode.Once;
		GetComponent<Animation>()["jog"].layer = 3; GetComponent<Animation>()["jog"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>()["laugh"].layer = 3; GetComponent<Animation>()["laugh"].wrapMode = WrapMode.Once;
		GetComponent<Animation>()["rally"].layer = 3; GetComponent<Animation>()["rally"].wrapMode = WrapMode.Once;
		GetComponent<Animation>()["strutwalk"].layer = 3; GetComponent<Animation>()["strutwalk"].wrapMode = WrapMode.Loop;
		GetComponent<Animation>()["talk"].layer = 3; GetComponent<Animation>()["talk"].wrapMode = WrapMode.Once;
		GetComponent<Animation>()["wave"].layer = 3; GetComponent<Animation>()["wave"].wrapMode = WrapMode.Once;
		
		GetComponent<Animation>().Play("weightshiftidle");
		
	}
	
	void FixedUpdate()
	{
			float targetMovementWeight = 0f;
			float throttle = 0f;
			
			// turning keys
			if (turningLeft) transform.Rotate(Vector3.down, turningSpeed*Time.deltaTime);
			if (turningRight) transform.Rotate(Vector3.up, turningSpeed*Time.deltaTime);
			
			// forward movement keys
			// ensure that the locomotion animations always blend from idle to moving at the beginning of their cycles
			if (startMovingForward && 
				(GetComponent<Animation>()["walk"].weight == 0f || GetComponent<Animation>()["run"].weight == 0f))
			{
				GetComponent<Animation>()["walk"].normalizedTime = 0f;
				GetComponent<Animation>()["run"].normalizedTime = 0f;
				startMovingForward = false;
				movingForward = true;
			}
			if (movingForward)
			{
				targetMovementWeight = MovementSpeed;
			}
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) throttle = 1f;
					
			// blend in the movement

			GetComponent<Animation>().Blend("run", targetMovementWeight*throttle, 0.5f);
			GetComponent<Animation>().Blend("walk", targetMovementWeight*(1f-throttle), 0.5f);
			// synchronize timing of the footsteps
			GetComponent<Animation>().SyncLayer(1);
			
			// all the other animations, such as punch, kick, attach, reaction, etc. go here
	//		if (Input.GetKeyDown(KeyCode.Alpha1)) animation.CrossFade("boredidle", 0.2f);
	//		if (Input.GetKeyDown(KeyCode.Alpha2)) animation.CrossFade("jog", 0.2f);
			if (laugh) {
							GetComponent<Animation>().CrossFade ("laugh", 0.2f);
							laugh = false;
					}
	//		if (Input.GetKeyDown(KeyCode.Alpha4)) animation.CrossFade("rally", 0.2f);
	//		if (Input.GetKeyDown(KeyCode.Alpha5)) animation.CrossFade("strutwalk", 0.2f);
			if (talk) 
			{
				GetComponent<Animation>().CrossFade ("talk", 0.2f);
				talk = false;
			}
			//if (Input.GetKeyDown(KeyCode.Alpha7)) animation.CrossFade("wave", 0.2f);
			
			this.pos = transform.position;
			this.forw = transform.forward;

			if (area != null) {
				Vector3 distance = transform.position - area.transform.position;

				if ((distance.x > area.bounds.extents.x*1.2 || distance.z > area.bounds.extents.z*1.2) && movingForward)
					AreaLimitReached(area.transform.position);
			}
	}

	void Update() {
		if (Time.timeScale == 1) {
			if (!GetComponent<Animation>().isPlaying)
				GetComponent<Animation>().Play();
		} else 
			GetComponent<Animation>().Stop();
	}

	void LateUpdate()
	{
		if (Time.timeScale == 1) {
			computer.ComputeRootMotion();
			
			// move the character using the computer's output
			character.SimpleMove(transform.TransformDirection(computer.deltaPosition)/Time.unscaledDeltaTime);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		this.area = other;
	}

	void OnTriggerExit(Collider other) {
		AreaLimitReached(other.transform.position);
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit){
		if (hit.moveDirection.normalized != Vector3.down && movingForward)
			ObstacleHit(hit);
	}

	void AreaLimitReached (Vector3 areaCenter)
	{
		if (Random.value > 0.5) TurnLeft(); else TurnRight();
		Thread t = new Thread(() => {
			Vector3 relativePosition = (areaCenter-pos).normalized;

			while (Vector3.Dot(forw,relativePosition) < 0.75f)
				Thread.Sleep(r.Next(10,50));

			MoveForward();
		});
		t.Start();
	}

	void ObstacleHit (ControllerColliderHit hit)
	{
		if (Random.value > 0.5) TurnLeft(); else TurnRight();
		Thread t = new Thread(() => {
			while (Vector3.Dot(forw,hit.normal) < 0)
				Thread.Sleep(r.Next(10,100));

			MoveForward();
		});
		t.Start();
	}

	System.Random r = new System.Random();
}