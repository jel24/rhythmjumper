using UnityEngine;
using System.Collections;

public class PlayerComponent : MonoBehaviour {

	PlayerStatusManager parent;

	void Start ()
	{
		parent = gameObject.GetComponentInParent<PlayerStatusManager>();
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Wall") {
			if (!coll.GetComponent<Collider2D> ().isTrigger) {
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
			if (!coll.GetComponent<Collider2D> ().isTrigger) {
				parent.StartTouching(gameObject.name);
			}
		}
	}

}
