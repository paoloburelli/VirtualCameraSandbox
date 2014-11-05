		
using UnityEngine;
using System.Collections;


[AddComponentMenu("Mixamo/Demo/Root Motion Character")]
public class RootMotionCharacterControlFEMALE: MonoBehaviour
{
	public float turningSpeed = 90f;
	public RootMotionComputer computer;
	public CharacterController character;
	public Vector3 DefaultMovement = Vector3.zero;
	
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
		animation["talking01"].layer = 0; animation["talking01"].wrapMode = WrapMode.Loop;
		animation["sexywalk"].layer = 1; animation["sexywalk"].wrapMode = WrapMode.Loop;
		animation["goofyrun"].layer = 1; animation["goofyrun"].wrapMode = WrapMode.Loop;
		animation["femtoughwalk"].layer = 3; animation["femtoughwalk"].wrapMode = WrapMode.Loop;
		animation["no"].layer = 3; animation["no"].wrapMode = WrapMode.Once;
		animation["sassywalk"].layer = 3; animation["sassywalk"].wrapMode = WrapMode.Loop;
		animation["standinggreeting"].layer = 3; animation["standinggreeting"].wrapMode = WrapMode.Once;
		animation["talking02"].layer = 3; animation["talking02"].wrapMode = WrapMode.Once;
		animation["tellsecret"].layer = 3; animation["tellsecret"].wrapMode = WrapMode.Once;
		
		//animation.Play("weightshiftidle");
		
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
		if (Input.GetKeyDown(KeyCode.W) && 
			(animation["sexywalk"].weight == 0f || animation["run"].weight == 0f))
		{
			animation["sexywalk"].normalizedTime = 0f;
			animation["goofyrun"].normalizedTime = 0f;
		}
		if (Input.GetKey(KeyCode.W))
		{
			targetMovementWeight = 1f;
		}
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) throttle = 1f;
				
		// blend in the movement

		animation.Blend("goofyrun", targetMovementWeight*throttle, 0.5f);
		animation.Blend("sexywalk", targetMovementWeight*(1f-throttle), 0.5f);
		// synchronize timing of the footsteps
		animation.SyncLayer(1);
		
		// all the other animations, such as punch, kick, attach, reaction, etc. go here
		if (Input.GetKeyDown(KeyCode.Alpha1)) animation.CrossFade("femtoughwalk", 0.2f);
		if (Input.GetKeyDown(KeyCode.Alpha2)) animation.CrossFade("no", 0.2f);
		if (Input.GetKeyDown(KeyCode.Alpha3)) animation.CrossFade("sassywalk", 0.2f);
		if (Input.GetKeyDown(KeyCode.Alpha4)) animation.CrossFade("standinggreeting", 0.2f);
		if (Input.GetKeyDown(KeyCode.Alpha5)) animation.CrossFade("talking02", 0.2f);
		if (Input.GetKeyDown(KeyCode.Alpha6)) animation.CrossFade("tellsecret", 0.2f);

	}
	
	void LateUpdate()
	{
		computer.ComputeRootMotion();
		
		// move the character using the computer's output
		if (DefaultMovement == Vector3.zero)
			character.SimpleMove(transform.TransformDirection(computer.deltaPosition)/Time.deltaTime);
		else
			character.SimpleMove(DefaultMovement);
	}
}