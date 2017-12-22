using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	public float jumpSpeed;
	public float jumpCooldown;
	public int maxBonusJumps;
	public AudioClip waterSound;
	public AudioClip missSound;
	public AudioClip hitSound;
	public delegate void OnJumpFunctionsDelegate ();
	public static OnJumpFunctionsDelegate jumpDelegate;

	public bool jumping;
	public ParticleTimer gracePrefab;
	public ParticleTimer waterPrefab;
	public ParticleTimer splashPrefab;
	public ParticleTimer phoenixPrefab;
	public ParticleTimer positivePrefab;
	public ParticleTimer negativePrefab;

	private MetronomeManager metronomeManager;
	private PlayerStatusManager statusManager;
	private ProgressManager progManager;

	private int jumps;
	private int bonusJumps;
	private float stun;
	private int metronomeCounter;
	private int tripletCounter;

	private Rigidbody2D rigidbody;
	private Animator animator;
	private SpriteRenderer renderer;
	private AudioSource audioSource;

	private bool onWallRight;
	private bool onWallLeft;
	private bool headTouching;
	private bool feetTouching;
	private bool inWater;
	private bool isSlippery;
	private bool tripletJumpUsed;

	private Vector2 waterAddedVelocity;
	private PowerupManager powerupManager;
	private PlayerCounter playerCounter;
	private JumpTypeManager jumpTypeManager;
	private int cancelCounter;


	float[] prevFrames = new float[3] {0, 0, 0};

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		renderer = GetComponent<SpriteRenderer>();
		statusManager = GetComponent<PlayerStatusManager>();
		audioSource = GetComponent<AudioSource> ();

		inWater = false;
		isSlippery = false;
		tripletJumpUsed = false;

		if (!statusManager) {
			Debug.Log ("Unable to find StatusManager!");
		}
		powerupManager = FindObjectOfType<PowerupManager> ();
		if (!powerupManager) {
			Debug.Log ("Unable to find PowerupManager!");
		}
		metronomeManager = FindObjectOfType<MetronomeManager> ();
		if (!metronomeManager) {
			Debug.Log ("Unable to find MetronomeManager!");
		}
		playerCounter = FindObjectOfType<PlayerCounter> ();
		if (!playerCounter) {
			Debug.Log ("Unable to find PlayerCounter!");
		}
		jumpTypeManager = FindObjectOfType<JumpTypeManager> ();
		if (!jumpTypeManager) {
			Debug.Log ("Unable to find JumpTypeManager!");
		}
		progManager = FindObjectOfType<ProgressManager> ();
		if (!progManager) {
			Debug.Log ("Unable to find ProgressManager.");
		}

		stun = 0f;
		jumpCooldown = 0.1f;
		jumpDelegate += OnJump;


	}
	
	// Update is called once per frame
	void LateUpdate ()
	{

		float inputX = CrossPlatformInputManager.GetAxis ("Horizontal");
		// Debug.Log (inputX);
		prevFrames [2] = prevFrames [1];
		prevFrames [1] = prevFrames [0];
		prevFrames [0] = inputX;

		if (stun > 0f) {
			stun -= Time.deltaTime;
		} else {
			stun  = 0f;
		}

		if (statusManager.IsAlive () && inWater) {
			ProcessWaterMovement ();

		} else if (statusManager.IsAlive () && !inWater) {
			ProcessStandardMovement ();

		} else {
			if (rigidbody.isKinematic == false && statusManager.IsAlive() == false) {
				rigidbody.isKinematic = true;
				rigidbody.velocity = Vector2.zero;
			}

		}
			
	}

	private float ProcessWaterMovement ()
	{
		float inputX = CrossPlatformInputManager.GetAxis ("Horizontal");
		float inputY = CrossPlatformInputManager.GetAxis ("Vertical");
		if (rigidbody.isKinematic) {
			rigidbody.isKinematic = false;
		}
		if (CrossPlatformInputManager.GetButtonDown ("Jump")) {
			animator.SetTrigger ("stroke");
			ParticleTimer waterFX = Instantiate (waterPrefab, gameObject.transform) as ParticleTimer;
			waterFX.GetComponent<ParticleTimer> ().SetExpiration (3f);
			waterFX.transform.position = transform.position;
			if (inputX > 0) {
				renderer.flipX = false;
			}
			else
				if (inputX < 0) {
					renderer.flipX = true;
				}
			if (powerupManager.HasBuff ("Grace")) {
				powerupManager.RemoveBuff ("Grace");
			}
			if (playerCounter.IsOnBeat ()) {
				playerCounter.Hit ();
				if (playerCounter.StreakStatus () >= 4) {
					playerCounter.ResetStreak ();
					powerupManager.AddBuff ("Grace");
					Invoke ("RemoveGraceBuff", 1f);
					rigidbody.velocity = new Vector2 (inputX * moveSpeed * 1.5f, inputY * moveSpeed * 1.5f);
				}
				else {
					rigidbody.velocity = new Vector2 (inputX * moveSpeed * 1.25f, inputY * moveSpeed * 1.25f);
				}
			}
			else {
				playerCounter.Miss ();
				rigidbody.velocity = new Vector2 (inputX * moveSpeed, inputY * moveSpeed * .50f);
				playerCounter.ResetStreak ();
			}
		}
		return inputX;
	}

	void ProcessStandardMovement ()
	{
		float inputX = CrossPlatformInputManager.GetAxis ("Horizontal");
		float inputY = CrossPlatformInputManager.GetAxis ("Vertical");
		if (rigidbody.isKinematic) {
			rigidbody.isKinematic = false;
		}
		if (CrossPlatformInputManager.GetButtonDown ("Jump") && !(stun < 0f)) {
			Jump ();
		}
		if (CrossPlatformInputManager.GetButtonDown ("SwapLeft") && !jumping) {
			jumpTypeManager.SwapLeft ();
		}

		if (CrossPlatformInputManager.GetButtonDown ("SwapRight") && !jumping) {
			jumpTypeManager.SwapRight ();
		}

		if (CrossPlatformInputManager.GetButtonDown ("Triplet") && progManager.HasUpgrade(Upgrade.TripletJump)) {
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
			playerCounter.AlecMode ();
		}

		if (!onWallRight && inputX >= 0 && !jumping && stun <= 0 || !onWallLeft && inputX <= 0 && !jumping && stun <= 0) {
				rigidbody.velocity = new Vector2 (moveSpeed * inputX, rigidbody.velocity.y);
			} 

			if (jumping && stun <= 0) {
				rigidbody.velocity = new Vector2 (moveSpeed * inputX, rigidbody.velocity.y);
			}

			if (!jumping) {
				animator.SetBool ("running", true);
			}
			if (inputX > 0) {
				renderer.flipX = false;
			} else if (inputX < 0) {
				renderer.flipX = true;
			}

		if (!jumping && Mathf.Abs (inputX) > 0) {
			animator.SetBool ("running", true);
		} else if (!jumping && inputX == 0) {
			animator.SetBool ("running", false);

		}
	}

	private void Jump ()
	{
		float jumpModifier;
		int maxJumps = 0;
		JumpType j = jumpTypeManager.getJumpType ();

		Debug.Log (j);

		switch (j) {
		case JumpType.Eighth:
			jumpModifier = .67f;
			maxJumps = 8;
			break;
		case JumpType.Quarter:
			jumpModifier = 1f;
			maxJumps = 4;
			break;
		case JumpType.Half:
			jumpModifier = 1.33f;
			maxJumps = 2;
			break;
		case JumpType.Whole:	
			jumpModifier = 2f;
			maxJumps = 1;
			break;
		default:
			print ("No jump types available.");
			jumpModifier = 1.0f;
			maxJumps = 1;
			break;
		}



		if (!jumping) {
			jumpDelegate ();
			jumping = true;

			if (playerCounter.IsOnBeat () || !metronomeManager.musicLevel) {
				playerCounter.Hit ();

				if (onWallLeft) {
					rigidbody.velocity = new Vector2 (5f, jumpSpeed * jumpModifier);
					stun += jumpCooldown;
				} else if (onWallRight) {
					rigidbody.velocity = new Vector2 (-5f, jumpSpeed * jumpModifier);
					stun += jumpCooldown;
				} else {
					rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed * jumpModifier);
					stun += jumpCooldown;
				}


				if (powerupManager.HasBuff ("MetronomeActive")){
					metronomeCounter++;

					if (metronomeCounter >= maxJumps){
						print ("Removing Metronome, expired.");
						powerupManager.RemoveBuff ("MetronomeActive");
					}
				}

			} else {
				playerCounter.Miss ();

				// Display negative particles
				ParticleTimer jumpFX = Instantiate (negativePrefab) as ParticleTimer;
				jumpFX.GetComponent<ParticleTimer>().SetExpiration(3f);
				jumpFX.transform.position = transform.position;
				//

				if (onWallLeft) {
					rigidbody.velocity = new Vector2 (3f, jumpSpeed * jumpModifier * .67f);
				} else if (onWallRight) {
					rigidbody.velocity = new Vector2 (-3f, jumpSpeed * jumpModifier * .67f);
				} else {
					rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed * jumpModifier * .67f);
				}
				stun += jumpCooldown;

			}

		} else if (jumping && bonusJumps > 0) {
			jumpDelegate ();
			animator.SetTrigger ("doubleJump");
			bonusJumps--;
			Debug.Log (bonusJumps + " jumps left.");


			if (playerCounter.IsOnBeat () && !playerCounter.ActiveBeatHitBefore() && stun == 0) {
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed * jumpModifier);
				stun += jumpCooldown;
				Debug.Log ("On beat!");
				playerCounter.Hit ();
				//Display positive particles
				ParticleTimer jumpFX = Instantiate (positivePrefab) as ParticleTimer;
				jumpFX.GetComponent<ParticleTimer>().SetExpiration(3f);
				jumpFX.transform.position = transform.position;
				//

				audioSource.clip = hitSound;
				audioSource.Play ();

				if (bonusJumps == 0) {
					playerCounter.LastJump ();
				}

				if (powerupManager.HasBuff ("MetronomeActive")){
					metronomeCounter++;

					if (metronomeCounter >= maxJumps){
						print ("Removing Metronome, expired.");
						powerupManager.RemoveBuff ("MetronomeActive");
					}
				}

			} else {
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x * .67f, jumpSpeed * jumpModifier * .67f);
				stun += jumpCooldown;
				playerCounter.Miss ();
				// Display negative particles
				ParticleTimer jumpFX = Instantiate (negativePrefab) as ParticleTimer;
				jumpFX.GetComponent<ParticleTimer>().SetExpiration(3f);
				jumpFX.transform.position = transform.position;
				//

				audioSource.clip = missSound;
				audioSource.Play ();

			}
		} else {
			Debug.Log ("Out of jumps!");
		}

	}

	private void TripletJump(){


		tripletCounter += 1;
		print (tripletCounter);
		if (tripletCounter == 2 && !tripletJumpUsed) {
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
			rigidbody.velocity = new Vector2 (rigidbody.velocity.x * 1.5f, jumpSpeed * jumpModifier * 1.5f);
		}
	}

	public void StartTouching (string name)
	{
		if (!inWater) {
			JumpType j = jumpTypeManager.getJumpType ();
			int refreshJumps = 0;

			switch (j) {
			case JumpType.Eighth:
				refreshJumps = 7;
				break;
			case JumpType.Quarter:
				refreshJumps = 3;
				break;
			case JumpType.Half:
				refreshJumps = 1;
				break;
			case JumpType.Whole:
				refreshJumps = 0;
				break;
			default:
				//print ("No jump types available.");
				break;
			}

			if (name == "Feet") {
				jumping = false;
				bonusJumps = refreshJumps;
				animator.SetBool ("jumping", false);
				animator.SetBool ("onwall", false);
				feetTouching = true;
				tripletJumpUsed = false;
			} else if (name == "Head") {
				headTouching = true;
			} else if (name == "Left" && !isSlippery) {
				onWallLeft = true;
				tripletJumpUsed = false;
				jumping = false;
				if (!headTouching) {
					bonusJumps = refreshJumps;
					//Debug.Log("Resetting jumps LEFT.");

				}
				if (!feetTouching) {
					animator.SetBool ("onwall", true);

				}
				animator.SetBool ("jumping", false);
				renderer.flipX = true;
			} else if (name == "Right" && !isSlippery) {
				onWallRight = true;
				jumping = false;
				tripletJumpUsed = false;

				if (!headTouching) {
					bonusJumps = refreshJumps;
					//Debug.Log("Resetting jumps RIGHT.");
				}
				if (!feetTouching) {
					animator.SetBool ("onwall", true);

				}
				animator.SetBool ("jumping", false);
				renderer.flipX = false;
			}

		}
	}

	public void EndTouching (string name)
	{
			if (name == "Feet") {
				animator.SetBool ("jumping", true);
				jumping = true;
				feetTouching = false;
			} else if (name == "Head") {
				headTouching = false;
			} else if (name == "Left") {
				onWallLeft = false;
				jumping = true;
				animator.SetBool("jumping", true);
				animator.SetBool("onwall", false);
				renderer.flipX = false;
			} else if (name == "Right") {
				onWallRight = false;
				jumping = true;
				animator.SetBool("jumping", true);
				animator.SetBool("onwall", false);
				renderer.flipX = true;
			}

	}

	private void UsePowerup(){
		if (powerupManager.HasBuff ("Metronome")) {
			metronomeCounter = 0;
			powerupManager.RemoveBuff ("Metronome");

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

			ParticleTimer phoenixFX = Instantiate (phoenixPrefab, gameObject.transform) as ParticleTimer;
			phoenixFX.GetComponent<ParticleTimer>().SetExpiration(4f);
			phoenixFX.transform.position = transform.position;
			bonusJumps = maxJumps;
			rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed * 2f);

		}
	}

	public void ChangeWaterStatus(bool isInWater){

		if (isInWater) {
			bonusJumps = 1;
			rigidbody.gravityScale = -.2f;
			rigidbody.drag = 2f;
			onWallRight = false;
			onWallLeft = false;
			feetTouching = false;
			headTouching = false;
			animator.SetBool ("inWater", true);
			ParticleTimer waterFX = Instantiate (splashPrefab) as ParticleTimer;
			waterFX.GetComponent<ParticleTimer>().SetExpiration(3f);
			waterFX.transform.position = transform.position;

		} else {
			bonusJumps = 1;
			animator.SetBool ("inWater", false);
			rigidbody.gravityScale = 1.25f;
			rigidbody.drag = 0f;
			if (powerupManager.HasBuff ("Grace")) {
				rigidbody.velocity = new Vector2 (0f, 13f);
				animator.SetTrigger ("grace");
				powerupManager.RemoveBuff ("Grace");
				print ("Graceful exit.");
				ParticleTimer waterFX = Instantiate (gracePrefab, gameObject.transform) as ParticleTimer;
				waterFX.GetComponent<ParticleTimer> ().SetExpiration (4f);
				waterFX.transform.position = transform.position;
				GetComponent<AudioSource> ().clip = waterSound;
				GetComponent<AudioSource> ().Play ();
			} else {
				rigidbody.AddForce(new Vector2 (0f, 2f));
			}
		}

		inWater = isInWater;

	}

	public void AddStun(float duration){
		stun += duration;
		Debug.Log ("Adding stun.");
	}

	public void ChangeSlipStatus(bool inSlipRegion){
		isSlippery = inSlipRegion;
	}

	private void RemoveGraceBuff(){
		powerupManager.RemoveBuff ("Grace");
	}

	public bool IsInWater(){
		return inWater;
	}

	public void AddFunctionToJump(OnJumpFunctionsDelegate method){
		jumpDelegate += method;
	}

	public void ResetTriplets(){
		tripletCounter = 0;
		//print ("Resetting triplets");
	}

	private void OnJump(){
		//Debug.Log ("Delegated!");
	}
}
