using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	public float jumpSpeed;
	public int jumpCooldownMax;
	public int maxBonusJumps;
	public AudioClip waterSound;
	public AudioClip missSound;
	public AudioClip hitSound;


	public bool jumping;
	public ParticleTimer gracePrefab;
	public ParticleTimer waterPrefab;
	public ParticleTimer splashPrefab;
	public ParticleTimer phoenixPrefab;
	public ParticleTimer positivePrefab;
	public ParticleTimer negativePrefab;

	private MetronomeManager metronomeManager;
	private PlayerStatusManager statusManager;

	private int jumps;
	private int bonusJumps;
	private int jumpCooldown;
	private int metronomeCounter;

	private Rigidbody2D rigidbody;
	private Animator animator;
	private SpriteRenderer renderer;
	private AudioSource audioSource;

	private bool onWallRight;
	private bool onWallLeft;
	private bool headTouching;
	private bool feetTouching;
	private bool inWater;

	private Vector2 waterAddedVelocity;
	private PowerupManager powerupManager;
	private PlayerCounter playerCounter;
	private JumpTypeManager jumpTypeManager;


	float[] prevFrames = new float[3] {0, 0, 0};

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		renderer = GetComponent<SpriteRenderer>();
		statusManager = GetComponent<PlayerStatusManager>();
		audioSource = GetComponent<AudioSource> ();
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
	}
	
	// Update is called once per frame
	void Update ()
	{

		float inputX = CrossPlatformInputManager.GetAxis ("Horizontal");
		// Debug.Log (inputX);
		prevFrames [2] = prevFrames [1];
		prevFrames [1] = prevFrames [0];
		prevFrames [0] = inputX;

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
		maxBonusJumps = 3;
		if (CrossPlatformInputManager.GetButtonDown ("Jump")) {
			Jump ();
		}
		if (CrossPlatformInputManager.GetButtonDown ("SwapLeft")) {
			jumpTypeManager.SwapLeft ();
		}

		if (CrossPlatformInputManager.GetButtonDown ("SwapRight")) {
			jumpTypeManager.SwapRight ();
		}

		if (CrossPlatformInputManager.GetButtonDown ("UsePowerup")) {
			UsePowerup ();
		}

		if (jumpCooldown > 0) {
			jumpCooldown--;
		}
		if (inputX != 0 && jumpCooldown == 0) {
			if (inputX > 0 && onWallRight) {
				// do nothing
			}
			else
				if (inputX < 0 && onWallLeft) {
					// do nothing
				}
				else {
					// Debug.Log (inputX * moveSpeed * Time.deltaTime);
					rigidbody.velocity = new Vector2 (inputX * moveSpeed, rigidbody.velocity.y);
				}
			if (!jumping) {
				animator.SetBool ("running", true);
			}
			if (inputX > 0) {
				renderer.flipX = false;
			}
			else
				if (inputX < 0) {
					renderer.flipX = true;
				}
		}
		else
			if (!jumping) {
				animator.SetBool ("running", false);
			}
	}

	private void Jump ()
	{
		float jumpModifier;
		JumpType j = jumpTypeManager.getJumpType ();

		switch (j) {
		case JumpType.Eighth:
			jumpModifier = .5f;
			break;
		case JumpType.Quarter:
			jumpModifier = 1f;
			break;
		case JumpType.Half:
			jumpModifier = 1.75f;
			break;
		case JumpType.Whole:	
			jumpModifier = 2.5f;
			break;
		default:
			print ("No jump types available.");
			jumpModifier = 1.0f;
			break;
		}

		if (!jumping) {

			jumping = true;

			if (onWallLeft) {
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x + 5f, jumpSpeed * jumpModifier);
				jumpCooldown += jumpCooldownMax;
			} else if (onWallRight) {
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x - 5f, jumpSpeed * jumpModifier);
				jumpCooldown += jumpCooldownMax;
			} else {
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed * jumpModifier);
				jumpCooldown += jumpCooldownMax;
			}

			if (playerCounter.IsOnBeat ()) {
				playerCounter.Hit ();

				if (powerupManager.HasBuff ("MetronomeActive")){
					metronomeCounter++;

					if (metronomeCounter >= 3){
						print ("Removing Metronome, expired.");
						powerupManager.RemoveBuff ("MetronomeActive");
					}
				}

			} else {
				playerCounter.Miss ();
			}

		} else if (jumping && bonusJumps > 0) {

			animator.SetTrigger ("doubleJump");
			bonusJumps--;
			Debug.Log (bonusJumps + " jumps left.");


			if (playerCounter.IsOnBeat () && !playerCounter.ActiveBeatHitBefore()) {
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed * jumpModifier);
				jumpCooldown += jumpCooldownMax;
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

					if (metronomeCounter >= 3){
						print ("Removing Metronome, expired.");
						powerupManager.RemoveBuff ("MetronomeActive");
					}
				}

			} else {
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x * .7f, jumpSpeed * jumpModifier * .3f);
				jumpCooldown += jumpCooldownMax;
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

	public void StartTouching (string name)
	{
		if (!inWater) {
			
			if (name == "Feet") {
				jumping = false;
				bonusJumps = maxBonusJumps;
				animator.SetBool ("jumping", false);
				animator.SetBool ("onwall", false);
				feetTouching = true;
			} else if (name == "Head") {
				headTouching = true;
			} else if (name == "Left") {
				onWallLeft = true;
				jumping = false;
				if (!headTouching) {
					bonusJumps = maxBonusJumps;
					//Debug.Log("Resetting jumps LEFT.");

				}
				if (!feetTouching) {
					animator.SetBool ("onwall", true);

				}
				animator.SetBool ("jumping", false);
				renderer.flipX = true;
			} else if (name == "Right") {
				onWallRight = true;
				jumping = false;
				if (!headTouching) {
					bonusJumps = maxBonusJumps;
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

			ParticleTimer phoenixFX = Instantiate (phoenixPrefab, gameObject.transform) as ParticleTimer;
			phoenixFX.GetComponent<ParticleTimer>().SetExpiration(4f);
			phoenixFX.transform.position = transform.position;
			bonusJumps = 3;
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
		
	private void RemoveGraceBuff(){
		powerupManager.RemoveBuff ("Grace");
	}

	public bool IsInWater(){
		return inWater;
	}
}
