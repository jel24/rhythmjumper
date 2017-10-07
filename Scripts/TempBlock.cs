using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBlock : MonoBehaviour {

	public int jumpsRequired;
	public int offset;

	private int ticker;

	// Use this for initialization
	void Start () {
		ticker = 0 - offset;
		FindObjectOfType<PlayerController> ().AddFunctionToJump (Jump);
	}
	
	void Jump(){
		ticker++;
		if (ticker == jumpsRequired) {
			gameObject.SetActive (!gameObject.active);
			ticker = 0;
		}
	}
}
