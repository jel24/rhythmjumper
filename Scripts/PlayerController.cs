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

		MovePlayer (inputX);

		if (CrossPlatformInputManager.GetButtonDown ("Jump") && !(stun < 0f)) {
			Jump ();
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
		float moveSpeed = 0f;

		if (Mathf.Abs (inputX) > 0 && Mathf.Abs (inputX) < .33) {
			moveSpeed = moveSpeeds [0];
		} else if (Mathf.Abs (inputX) > .33 && Mathf.Abs (inputX) < .66) {
			moveSpeed = moveSpeeds [1];
		} else if (Mathf.Abs (inputX) > .67) {
			moveSpeed = moveSpeeds [2];
		}

		if (inputX < 0) {
			moveSpeed *= -1;
		}

		rigidbody.velocity = new Vector2 (moveSpeed, rigidbody.velocity.y);
	}

	private void Jump ()
	{
		rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed);
	}

	private void TripletJump(){

		/*
		tripletCounter += 1;
		print (tripletCounter);
		if (tripletCounter == 2 && !tripletJumpUsed && playerCounter.ActiveBeatHitBefore()) {
			tripletJumpUsed = true;
			float jumpModifier;
			JumpType j = jumpTypeManager.getJumpType ();

			switch (j) {
			case JumpType.Eighth:
				jumpModifier = .67f;
				break;
			case JumpType.Quarter:
				jumpModifier = 1f;
				break;
			case JumpType.Half:
				jumpModifier = 1.33f;
				break;
			case JumpType.Whole:	
				jumpModifier = 2f;
				break;
			default:
				print ("No jump types available.");
				jumpModifier = 1.0f;
				break;
			}

			bonusJumps = 0;
			tripletCounter = 0;
			AddParticlesOnPlayer (ParticleType.Triplet);

			rigidbody.velocity = new Vector2 (0f, jumpSpeed * 1.65f);
			animator.SetTrigger ("grace");
		}

*/
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
		Debug.Log ("Adding stun.");
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
