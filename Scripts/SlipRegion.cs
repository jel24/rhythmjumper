using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipRegion : MonoBehaviour {

	BoxCollider2D boxCollider;


	void OnDrawGizmos(){
		Gizmos.color = new Color (0f, 100f, 0f, .5f);
		boxCollider = GetComponent<BoxCollider2D> ();
		Gizmos.DrawCube(transform.position, new Vector3(boxCollider.size.x, boxCollider.size.y, 1f));
	}

	void OnTriggerEnter2D(Collider2D coll)
	{

		if (coll.gameObject.tag == "Player") {
			coll.GetComponentInParent<PlayerController> ().ChangeSlipStatus (true);
		} 
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			coll.GetComponentInParent<PlayerController> ().ChangeSlipStatus (false);
		} 
	}

}
