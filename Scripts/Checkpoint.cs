using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (transform.position, 1);
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			coll.gameObject.GetComponentInParent<PlayerStatusManager> ().startLocation = transform.position;
		}
	}
}
