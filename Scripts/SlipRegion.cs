using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipRegion : MonoBehaviour {

	private BoxCollider2D boxCollider;

	private ParticleSystem particles;


	void OnDrawGizmos(){
		Gizmos.color = new Color (0f, 100f, 0f, .5f);
		Gizmos.matrix = transform.localToWorldMatrix;
		boxCollider = GetComponent<BoxCollider2D> ();
		Gizmos.DrawCube(Vector3.zero, new Vector3(boxCollider.size.x, boxCollider.size.y, 1f));
	}

	void Start(){
		boxCollider = GetComponent<BoxCollider2D> ();
		particles = GetComponent<ParticleSystem> ();
		ParticleSystem.ShapeModule particleShape = particles.shape;
		particleShape.scale = new Vector3 (boxCollider.size.x, boxCollider.size.y, 0f);
	}


	void OnTriggerEnter2D(Collider2D coll)
	{

		if (coll.gameObject.tag == "Player") {
			coll.GetComponentInParent<PlayerStatusManager> ().ChangeSlipStatus (true);
		} 
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			coll.GetComponentInParent<PlayerStatusManager> ().ChangeSlipStatus (false);
		} 
	}

}
