using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("Exit", 8f);
	}

	private void Exit(){
		Application.Quit ();
	}

}
