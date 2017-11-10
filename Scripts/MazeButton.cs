using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeButton : MonoBehaviour {

	public MazeDirection direction;
	public Maze maze;

	void OnCollisionStay2D(Collision2D coll)
	{
		

		if (coll.collider.gameObject.tag == "Player") {
			Debug.Log ("Adding.");
			maze.SetDirection (direction);
		}
	}
}
