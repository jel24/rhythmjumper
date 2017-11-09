using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeDirection
{
	left,
	right,
	still
}

public class Maze : MonoBehaviour {

	private MazeDirection direction;



	// Use this for initialization
	void Start () {
		direction = MazeDirection.still;
	}
	
	// Update is called once per frame
	void Update () {

		switch (direction) {
		case MazeDirection.still:
			break;
		case MazeDirection.right:
			this.transform.Rotate (0f, 0f, .30f);
			break;
		case MazeDirection.left:
			this.transform.Rotate (0f, 0f, -.30f);
			break;
		}
	}

	public void SetDirection(MazeDirection d){
		direction = d;
	}
}
