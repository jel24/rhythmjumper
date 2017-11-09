using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeButton : MonoBehaviour {

	public MazeDirection direction;
	public Maze maze;

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			maze.SetDirection (direction);
		}
	}
}
