using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeButton : MonoBehaviour {

	public MazeDirection direction;
	public Maze maze;
	private bool compressed;
	private Vector3 loweredPosition;
	private Vector3 originalPosition;



	void Start(){
		compressed = false;
		loweredPosition = new Vector3 (transform.position.x, transform.position.y - .1f, transform.position.z);
		originalPosition = transform.position;

	}

	void OnTriggerStay2D(Collider2D coll)
	{
		

		if (coll.gameObject.tag == "Player") {
			//Debug.Log ("Adding.");
			maze.SetDirection (direction);
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player") {
			print ("Enter");
			if (!compressed) {
				compressed = true;
				transform.position = loweredPosition;
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll) {

		if (coll.gameObject.tag == "Player") {
			print ("Exit");

			if (compressed) {
				compressed = false;
				transform.position = originalPosition;
			}
		}
	}
}
