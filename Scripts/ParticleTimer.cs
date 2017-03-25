using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTimer : MonoBehaviour {

	public void SetExpiration(float timedLife){
		Invoke ("DestroyEmitter", timedLife);
	}

	private void DestroyEmitter(){
		Destroy (gameObject);
	}

}
