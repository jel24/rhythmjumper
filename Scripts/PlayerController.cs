using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	public float jumpSpeed;
	//public int jumpFrameLimit;
	public int jumpCooldownMax;
	public int maxBonusJumps;
	public MetronomeManager metronomeManager;

	private PlayerStatusManager statusManager;

	private int jumps;
	private int bonusJumps;
	private Rigidbody2D rigidbody;
	private Animator animator;
	private SpriteRenderer renderer;

	public bool jumping;
	private int jumpCooldown;
	private bool onWallRight;
	private bool onWallLeft;
	private bool headTouching;
	private bool feetTouching;

	float[] prevFrames = new float[3] {0, 0, 0};

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		renderer = GetComponent<SpriteRenderer>();
		statusManager = GetComponent<PlayerStatusManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{

		float inputX = CrossPlatformInputManager.GetAxis ("Horizontal");
		// Debug.Log (inputX);
		prevFrames [2] = prevFrames [1];
		prevFrames [1] = prevFrames [0];
		prevFrames [0] = inputX;

		if (statusManager.IsAlive ()) {
			if (rigidbody.isKinematic = false) {
				rigidbody.isKinematic = true;
			}

			if (statusManager.HasBuff("Metronome")){
				maxBonusJumps = 3;
			} else if (statusManager.HasBuff("Streak")){
				maxBonusJumps = 2;
			} else {
				maxBonusJumps = 1;
			}
		
			if (CrossPlatformInputManager.GetButtonDown ("Fire1")) {
				Jump ();
			}

			if (jumpCooldown > 0) {
				jumpCooldown--;
			}

			if (inputX != 0 && jumpCooldown == 0) {
				if (inputX > 0 && onWallRight) {
					// do nothing
				} else if (inputX < 0 && onWallLeft) {
					// do nothing
				} else {
					// Debug.Log (inputX * moveSpeed * Time.deltaTime);
					rigidbody.velocity = new Vector2 (inputX * moveSpeed, rigidbody.velocity.y);
				}

				if (!jumping) {
					animator.SetBool ("running", true);
				}
				if (inputX > 0) {
					renderer.flipX = false;
				} else if (inputX < 0) {
					renderer.flipX = true;
				}
			} else if (!jumping) {
				animator.SetBool ("running", false);
			}
		} else {
			if (rigidbody.isKinematic == false && statusManager.IsAlive() == false) {
				rigidbody.isKinematic = true;
				rigidbody.velocity = Vector2.zero;
			}

		}


		//rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, 5f);

	}

	private void Jump ()
	{

		if (!jumping) {

			jumping = true;
			
			if (onWallLeft) {
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x + 5f, jumpSpeed);
				jumpCooldown += jumpCooldownMax;
			} else if (onWallRight) {
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x - 5f, jumpSpeed);
				jumpCooldown += jumpCooldownMax;
			} else {
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed);
				jumpCooldown += jumpCooldownMax;
			}

			if (metronomeManager.IsOnBeat ()) {
				metronomeManager.AddStreak ();
			} else {
				metronomeManager.EndStreak ();
			}

		} else if (jumping && bonusJumps > 0) {
			bonusJumps--;
			Debug.Log (bonusJumps + " jumps left.");
			rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed);
			jumpCooldown += jumpCooldownMax;

			if (metronomeManager.IsOnBeat ()) {
				metronomeManager.AddStreak ();
			} else {
				metronomeManager.EndStreak ();
			}
		} else {
			Debug.Log ("Out of jumps!");
		}

	}

	public void StartTouching (string name)
	{
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
				animator.SetBool("onwall", true);
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
				animator.SetBool("onwall", true);
			}
			animator.SetBool("jumping", false);
			renderer.flipX = false;
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

}
