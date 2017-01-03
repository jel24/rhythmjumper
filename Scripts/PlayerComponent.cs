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
			//Debug.Log(gameObject.name);
			parent.StartTouching(gameObject.name);
			//jumping = false;
			//animator.SetBool("jumping", false);
		}
	}

	void OnTriggerExit2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Wall") {
			//Debug.Log(gameObject.name);
			parent.StartTouching(gameObject.name);
			//jumping = false;
			//animator.SetBool("jumping", false);
		}
	}

	void OnTriggerStay2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Wall") {
			//Debug.Log(gameObject.name);
			parent.StartTouching(gameObject.name);
			//jumping = false;
			//animator.SetBool("jumping", false);
		}
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Wall") {
			//Debug.Log(gameObject.name);
			parent.StartTouching(gameObject.name);
			//jumping = false;
			//animator.SetBool("jumping", false);
		}
	}

	void OnCollisionStay2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Wall") {
			//Debug.Log(gameObject.name);
			parent.StartTouching(gameObject.name);
			//jumping = false;
			//animator.SetBool("jumping", false);
		}
	}

	void OnCollisionExit2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Wall") {
			//Debug.Log(gameObject.name);
			parent.EndTouching(gameObject.name);
			//jumping = false;
			//animator.SetBool("jumping", false);
		}
	}
}
