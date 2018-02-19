using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearanceManager : MonoBehaviour {

	public AudioClip waterSound;
	public AudioClip missSound;
	public AudioClip hitSound;
	public AudioClip[] jumpSounds;

	private ParticleManager particleManager;

	private Animator animator;
	private SpriteRenderer renderer;
	private AudioSource audioSource;
	private Rigidbody2D rigidbody;


	// Use this for initialization
	void Start () {
		particleManager = FindObjectOfType<ParticleManager> ();
		if (!particleManager) {
			Debug.Log ("Unable to find ParticleManager.");
		}

		rigidbody = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		renderer = GetComponent<SpriteRenderer> ();

	}


	public void AddParticlesOnPlayer(ParticleType p){
		Instantiate (particleManager.GetParticle (p), transform.position, Quaternion.identity, gameObject.transform);

	}

	public void AddParticlesAtPlayer(ParticleType p){
		Instantiate (particleManager.GetParticle (p), transform.position, Quaternion.identity, particleManager.transform);

	}

	public void DoubleJump(){
		animator.SetTrigger ("doubleJump");
		UpdateSpriteDirection ();

	}

	public void Hit(){
		AddParticlesOnPlayer (ParticleType.JumpSuccess);

	}

	public void Miss(){
		AddParticlesOnPlayer (ParticleType.JumpFailure);

	}

	public void TripletJump(){
		AddParticlesOnPlayer (ParticleType.Triplet);
		animator.SetTrigger ("grace");
	}

	public void Phoenix(){
		AddParticlesOnPlayer (ParticleType.Phoenix);
		animator.SetTrigger ("grace");
	}

	public void UpdateAppearanceFromMovementState(MovementState s){
		switch (s) {
		case MovementState.Grounded:
			animator.SetBool ("jumping", false);
			animator.SetBool ("onwall", false);
			if (Mathf.Abs (rigidbody.velocity.x) > 0) {
				animator.SetBool ("running", true);
			} else {
				animator.SetBool ("running", false);
			}
			UpdateSpriteDirection ();
			break;
		case MovementState.OnWallLeft:
			animator.SetBool ("onwall", true);
			animator.SetBool ("running", false);
			animator.SetBool ("jumping", false);
			renderer.flipX = true;
			AddParticlesAtPlayer (ParticleType.WallDustLeft);

			break;
		case MovementState.OnWallRight:
			animator.SetBool ("onwall", true);
			animator.SetBool ("running", false);
			animator.SetBool ("jumping", false);
			renderer.flipX = false;
			AddParticlesAtPlayer (ParticleType.WallDustRight);
			break;
		case MovementState.Jumping:
			UpdateSpriteDirection ();
			animator.SetBool ("jumping", true);
			animator.SetBool ("onwall", false);
			animator.SetBool ("running", false);
			break;
		default:
			break;
		}
	}


	private void UpdateSpriteDirection(){
		if (rigidbody.velocity.x < 0) {
			renderer.flipX = true;
		} else if (rigidbody.velocity.x > 0) {
			renderer.flipX = false;
		}
	}
}


 				//if (!feetTouching) {
					//animator.SetBool ("onwall", true);
					//AddParticlesAtPlayer (ParticleType.WallDustRight);

				//}
