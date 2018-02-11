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

	private void AddParticlesOnPlayer(ParticleType p){
		Instantiate (particleManager.GetParticle (p), transform.position, Quaternion.identity, gameObject.transform);

	}

	private void AddParticlesAtPlayer(ParticleType p){
		Instantiate (particleManager.GetParticle (p), transform.position, Quaternion.identity);

	}

	public void UpdateAppearanceFromMovementState(MovementState s){
		print (s);
		switch (s) {
		case MovementState.Grounded:
			print ("This is happening.");
			animator.SetBool ("jumping", false);
			animator.SetBool ("onwall", false);
			if (Mathf.Abs (rigidbody.velocity.x) > 0) {
				animator.SetBool ("running", true);
			} else {
				animator.SetBool ("running", false);
			}

			if (rigidbody.velocity.x < 0) {
				renderer.flipX = true;
			} else if (rigidbody.velocity.x > 0) {
				renderer.flipX = false;

			}

			break;
		case MovementState.OnWall:
			animator.SetBool ("onwall", true);
			animator.SetBool ("running", false);
			animator.SetBool ("jumping", false);

			break;
		case MovementState.Jumping:
			animator.SetBool ("jumping", true);
			animator.SetBool ("onwall", false);
			animator.SetBool ("running", false);

			break;
		default:
			break;
		}
	}

}


 				//if (!feetTouching) {
					//animator.SetBool ("onwall", true);
					//AddParticlesAtPlayer (ParticleType.WallDustRight);

				//}
