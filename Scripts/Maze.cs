﻿using System.Collections;
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
	private float magnitude;


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
			this.transform.Rotate (0f, 0f, magnitude);
			break;
		case MazeDirection.left:
			this.transform.Rotate (0f, 0f, -magnitude);
			break;
		}
	}

	public void SetDirection(MazeDirection d){
		if (direction == MazeDirection.left) {
			magnitude -= .05f;
		} else if (direction == MazeDirection.right) {
			magnitude += .05f;
		}
	}
}
