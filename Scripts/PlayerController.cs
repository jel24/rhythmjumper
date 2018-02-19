using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour {

	public float moveSpeedMax;
	public float jumpSpeed;
	public float jumpCooldown;
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

		if (statusManager.IsAlive ()) {
			ProcessMovement ();
		}

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

		float rate = .5f;
		float moveSpeed = rigidbody.velocity.x;

		switch (statusManager.CurrentState) {
		case MovementState.OnWallLeft:
		case MovementState.OnWallRight:
			moveSpeed = rigidbody.velocity.x + rate * inputX;
			moveSpeed = Mathf.Clamp (moveSpeed, moveSpeedMax * -1, moveSpeedMax);
			//float fallSpeed = Mathf.Clamp (rigidbody.velocity.y, rigidbody.velocity.y, -1.5f);
			rigidbody.velocity = new Vector2 (moveSpeed, -2f);
			break;
		/*case MovementState.Jumping:
			moveSpeed = rigidbody.velocity.x + rate * inputX * 5;
			moveSpeed = Mathf.Clamp (moveSpeed, moveSpeedMax * -1, moveSpeedMax);
			rigidbody.velocity = new Vector2 (moveSpeed, rigidbody.velocity.y);
			break;*/
		default:
			moveSpeed = rigidbody.velocity.x + rate * inputX;
			moveSpeed = Mathf.Clamp (moveSpeed, moveSpeedMax * -1, moveSpeedMax);
			rigidbody.velocity = new Vector2 (moveSpeed, rigidbody.velocity.y);
			break;
		}

	}

	private void Jump ()
	{
		if (statusManager.CanJumpAgain()){
			OnJump ();
			AddStun (jumpCooldown);
			switch (statusManager.CurrentState) {
			case MovementState.OnWallLeft:
				if (statusManager.UseJumpOnBeat ()) {
					rigidbody.velocity = new Vector2 (5f, jumpSpeed);
				} else {
					rigidbody.velocity = new Vector2 (5f * .5f, jumpSpeed * .67f);
				}
				break;
			case MovementState.OnWallRight:
				if (statusManager.UseJumpOnBeat ()) {
					rigidbody.velocity = new Vector2 (-5f, jumpSpeed);
				} else {
					rigidbody.velocity = new Vector2 (-5f * .5f, jumpSpeed * .67f);
				}
				break;
			default:
				if (statusManager.UseJumpOnBeat ()) {
					rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed);
				} else {
					rigidbody.velocity = new Vector2 (rigidbody.velocity.x * .5f, jumpSpeed  * .67f);
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
		if (statusManager.Phoenix ()) {
			rigidbody.velocity = new Vector2 (0f, jumpSpeed * 2.25f);
			AddStun (1f);
		}
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
