using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimation : MonoBehaviour {

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		Invoke("StartAnimating", Random.Range(0f, 3f));
	}

	private void StartAnimating(){
		animator.SetTrigger ("start");
	}
}
