using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTimer : MonoBehaviour {

	void Start(){
		Invoke ("DestroyEmitter", 4f);
	}

	private void DestroyEmitter(){
		Destroy (gameObject);
	}

}
