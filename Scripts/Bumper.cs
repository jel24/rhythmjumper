using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour {

	public Vector2 force;

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			coll.GetComponentInParent<PlayerController> ().AddStun (.33f);
		}
	}

	void OnTriggerStay2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			coll.GetComponentInParent<Rigidbody2D>().AddForce (force);
		}
	}
}
