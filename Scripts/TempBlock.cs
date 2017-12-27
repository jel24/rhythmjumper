using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBlock : MonoBehaviour {

	public int jumpsRequired;
	public int offset;

	private int ticker;
	private BoxCollider2D hitbox;
	private Animator anim;

	// Use this for initialization
	void Start () {
		ticker = 0 - offset;
		hitbox = GetComponent<BoxCollider2D> ();
		anim = GetComponent<Animator> ();
		FindObjectOfType<PlayerController> ().AddFunctionToJump (Jump);
	}
	
	void Jump(){
		ticker++;
		if (ticker == jumpsRequired) {
			//gameObject.SetActive (!gameObject.active);
			hitbox.isTrigger = !hitbox.isTrigger;
			if (hitbox.isTrigger) {
				anim.SetBool ("Hidden", true);
			} else {
				anim.SetBool ("Hidden", false);
			}
			ticker = 0;
		}
	}
}
