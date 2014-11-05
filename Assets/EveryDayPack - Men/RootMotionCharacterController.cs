		
using UnityEngine;
using System.Collections;


[AddComponentMenu("Mixamo/Demo/Root Motion Character")]
public class RootMotionCharacterControlMENS : MonoBehaviour
{
	public float turningSpeed = 90f;
	public RootMotionComputer computer;
	public CharacterController character;

	public void MoveForward() {
		Stop ();
		startMovingForward = true;
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
		animation["weightshiftidle"].layer = 0; animation["weightshiftidle"].wrapMode = WrapMode.Loop;
		animation["walk"].layer = 1; animation["walk"].wrapMode = WrapMode.Loop;
		animation["run"].layer = 1; animation["run"].wrapMode = WrapMode.Loop;
		animation["boredidle"].layer = 3; animation["boredidle"].wrapMode = WrapMode.Once;
		animation["jog"].layer = 3; animation["jog"].wrapMode = WrapMode.Loop;
		animation["laugh"].layer = 3; animation["laugh"].wrapMode = WrapMode.Once;
		animation["rally"].layer = 3; animation["rally"].wrapMode = WrapMode.Once;
		animation["strutwalk"].layer = 3; animation["strutwalk"].wrapMode = WrapMode.Loop;
		animation["talk"].layer = 3; animation["talk"].wrapMode = WrapMode.Once;
		animation["wave"].layer = 3; animation["wave"].wrapMode = WrapMode.Once;
		
		animation.Play("weightshiftidle");
		
	}
	
	void Update()
	{
		float targetMovementWeight = 0f;
		float throttle = 0f;
		
		// turning keys
		if (Input.GetKey(KeyCode.A)) transform.Rotate(Vector3.down, turningSpeed*Time.deltaTime);
		if (Input.GetKey(KeyCode.D)) transform.Rotate(Vector3.up, turningSpeed*Time.deltaTime);
		
		// forward movement keys
		// ensure that the locomotion animations always blend from idle to moving at the beginning of their cycles
		if (startMovingForward && 
			(animation["walk"].weight == 0f || animation["run"].weight == 0f))
		{
			animation["walk"].normalizedTime = 0f;
			animation["run"].normalizedTime = 0f;
			startMovingForward = false;
			movingForward = true;
		}
		if (movingForward)
		{
			targetMovementWeight = 1f;
		}
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) throttle = 1f;
				
		// blend in the movement

		animation.Blend("run", targetMovementWeight*throttle, 0.5f);
		animation.Blend("walk", targetMovementWeight*(1f-throttle), 0.5f);
		// synchronize timing of the footsteps
		animation.SyncLayer(1);
		
		// all the other animations, such as punch, kick, attach, reaction, etc. go here
//		if (Input.GetKeyDown(KeyCode.Alpha1)) animation.CrossFade("boredidle", 0.2f);
//		if (Input.GetKeyDown(KeyCode.Alpha2)) animation.CrossFade("jog", 0.2f);
		if (laugh) {
						animation.CrossFade ("laugh", 0.2f);
						laugh = false;
				}
//		if (Input.GetKeyDown(KeyCode.Alpha4)) animation.CrossFade("rally", 0.2f);
//		if (Input.GetKeyDown(KeyCode.Alpha5)) animation.CrossFade("strutwalk", 0.2f);
		if (talk) 
		{
			animation.CrossFade ("talk", 0.2f);
			talk = false;
		}
		//if (Input.GetKeyDown(KeyCode.Alpha7)) animation.CrossFade("wave", 0.2f);
		
		
	}
	
	void LateUpdate()
	{
		computer.ComputeRootMotion();
		
		// move the character using the computer's output
		character.SimpleMove(transform.TransformDirection(computer.deltaPosition)/Time.deltaTime);
	}
}