using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTimer : MonoBehaviour {

	public float DestroyTimer;

	void Start(){
		if (DestroyTimer > 0f) {
			Invoke ("DestroyEmitter", DestroyTimer);
		} else {
			Invoke ("DestroyEmitter", 4f);

		}
	}

	private void DestroyEmitter(){
		Destroy (gameObject);
	}

}
