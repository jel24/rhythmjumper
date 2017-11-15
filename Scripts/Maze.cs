using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeDirection
{
	left,
	right,
}

public class Maze : MonoBehaviour {

	private float magnitude;


	private int consecutive;
	private PlayerCounter playerCounter;
	private bool puzzleBeaten;

	// Use this for initialization
	void Start () {
		playerCounter = FindObjectOfType<PlayerCounter> ();
		puzzleBeaten = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (magnitude != 0) {
			this.transform.Rotate (0f, 0f, magnitude);

		}

	}

	public void SetDirection(MazeDirection d){
		if (d == MazeDirection.left) {
			magnitude -= .01f;
			if (magnitude < -3f) {
				magnitude = -3f;
			}
		} else if (d == MazeDirection.right) {
			magnitude += .01f;
			if (magnitude > 3f) {
				magnitude = 3f;
			}
		}
	}

	public void BellRung(){
		if (playerCounter.IsOnBeat()) {
			playerCounter.Hit ();
			consecutive += 1;
			Debug.Log ("Bell rung on beat, " + consecutive);
			if (consecutive >= 8 && !puzzleBeaten) {
				Debug.Log ("Puzzle passed.");
				puzzleBeaten = true;
				gameObject.SetActive (false);
				//puzzle passed
			}
		} else {
			consecutive = 0;
			Debug.Log ("Bell rung off beat.");

		}
	}
}
