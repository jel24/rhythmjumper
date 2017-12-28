using UnityEngine;
using System.Collections;

public class PlayerComponent : MonoBehaviour {

	PlayerController parent;

	void Start ()
	{
		parent = gameObject.GetComponentInParent<PlayerController>();
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Wall") {
			if (!coll.GetComponent<BoxCollider2D> ().isTrigger) {
				parent.StartTouching(gameObject.name);
			}
		}
	}

	void OnTriggerExit2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Wall") {
			parent.EndTouching(gameObject.name);
		}
	}

	void OnTriggerStay2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Wall") {
			if (!coll.GetComponent<BoxCollider2D> ().isTrigger) {
				parent.StartTouching(gameObject.name);
			}
		}
	}

}
