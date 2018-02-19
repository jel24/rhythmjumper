using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour {

	public float[] moveSpeeds;
	public float jumpSpeed;
	public float jumpCooldown;
	public int maxBonusJumps;
	public delegate void OnJumpFunctionsDelegate ();
	public static OnJumpFunctionsDelegate jumpDelegate;

	private MetronomeManager metronomeManager;
	private PlayerStatusManager statusManager;
	private float stun;
	private Rigidbody2D rigidbody;
	private int cancelCounter;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody2D>();
		statusManager = GetComponent<PlayerStatusManager>();

		if (!statusManager) {
			Debug.Log ("Unable to find StatusManager!");
		}

		metronomeManager = FindObjectOfType<MetronomeManager> ();
		if (!metronomeManager) {
			Debug.Log ("Unable to find MetronomeManager!");
		}
			
		stun = 0f;
		jumpDelegate += OnJump;

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (stun > 0f) {
			stun -= Time.deltaTime;
		} else {
			stun  = 0f;
		}
		ProcessMovement ();

	}


	private void ProcessMovement ()
	{
		float inputX = CrossPlatformInputManager.GetAxis ("Horizontal");
		float inputY = CrossPlatformInputManager.GetAxis ("Vertical");

		if (!(stun > 0f)) {
			
			MovePlayer (inputX);

			if (CrossPlatformInputManager.GetButtonDown ("Jump")) {
				Jump ();
			}
		}



		if (CrossPlatformInputManager.GetButtonDown ("SwapLeft")) {
			//jumpTypeManager.SwapLeft ();
		}
	
		if (CrossPlatformInputManager.GetButtonDown ("SwapRight")) {
			//jumpTypeManager.SwapRight ();
		}

		if (CrossPlatformInputManager.GetButtonDown ("Triplet")) {
			TripletJump ();
		}

		if (CrossPlatformInputManager.GetButton ("Cancel")) {
			cancelCounter++;
			print(cancelCounter);
			if (cancelCounter >= 180){
				SceneManager.LoadScene("menu");
			}
		} else {
			cancelCounter = 0;
		}

		if (CrossPlatformInputManager.GetButtonDown ("UsePowerup")) {
			UsePowerup ();
		}
			
		if (CrossPlatformInputManager.GetButtonDown ("AlecButton")) {
			//playerCounter.AlecMode ();
		}
			
	}

	private void MovePlayer(float inputX){

		float delta = inputX * .5f;
		float moveSpeed = rigidbody.velocity.x + delta;

		switch (statusManager.CurrentState) {
		case MovementState.OnWallLeft:
		case MovementState.OnWallRight:
			moveSpeed = Mathf.Clamp (moveSpeed, moveSpeeds [2] * -1, moveSpeeds [2]);
			//float fallSpeed = Mathf.Clamp (rigidbody.velocity.y, rigidbody.velocity.y, -1.5f);
			rigidbody.velocity = new Vector2 (moveSpeed, -1.5f);
			break;
			break;
		default:
			moveSpeed = Mathf.Clamp (moveSpeed, moveSpeeds [2] * -1, moveSpeeds [2]);
			rigidbody.velocity = new Vector2 (moveSpeed, rigidbody.velocity.y);
			break;
		}

	}

	private void Jump ()
	{
		if (statusManager.CanJumpAgain()){
			AddStun (jumpCooldown);
			switch (statusManager.CurrentState) {
			case MovementState.OnWallLeft:
				if (statusManager.UseJumpOnBeat ()) {
					rigidbody.velocity = new Vector2 (5f, jumpSpeed);
				} else {
					rigidbody.velocity = new Vector2 (5f * .5f, jumpSpeed * .5f);
				}
				break;
			case MovementState.OnWallRight:
				if (statusManager.UseJumpOnBeat ()) {
					rigidbody.velocity = new Vector2 (-5f, jumpSpeed);
				} else {
					rigidbody.velocity = new Vector2 (-5f * .5f, jumpSpeed * .5f);
				}
				break;
			default:
				if (statusManager.UseJumpOnBeat ()) {
					rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed);
				} else {
					rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed  * .25f);
				}
				break;

			}




		}
	}

	private void TripletJump(){
		if (statusManager.TripletJump()) {
			rigidbody.velocity = new Vector2 (0f, jumpSpeed * 1.65f);
			AddStun (.5f);
		}
	}

	private void UsePowerup(){
		/*if (powerupManager.HasPowerup(Powerup.Phoenix)) {
			metronomeCounter = 0;
			powerupManager.UsePowerup (Powerup.Phoenix);

			JumpType j = jumpTypeManager.getJumpType ();
			int maxJumps = 0;

			switch (j) {
			case JumpType.Eighth:
				maxJumps = 8;
				break;
			case JumpType.Quarter:
				maxJumps = 4;
				break;
			case JumpType.Half:
				maxJumps = 2;
				break;
			case JumpType.Whole:	
				maxJumps = 1;
				break;
			default:
				print ("No jump types available.");
				break;
			}

			AddParticlesOnPlayer (ParticleType.Phoenix);

			bonusJumps = maxJumps;
			rigidbody.velocity = new Vector2 (0f, jumpSpeed * 2.25f);
			animator.SetTrigger ("grace");
			AddStun (1f);
		}*/
	}


	public void AddStun(float duration){
		stun += duration;
		//Debug.Log ("Adding stun.");
	}
		

	public void AddFunctionToJump(OnJumpFunctionsDelegate method){
		jumpDelegate += method;
	}
		
	private void OnJump(){
		//audioSource.clip = jumpSounds[Random.Range(0, jumpSounds.Length-1)];
		//audioSource.Play ();

		//Debug.Log ("Delegated!");
	}
}
