using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour {

	private AudioSource audioSource;

	public Maze maze;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		Debug.Log (coll.name);
		if (coll.GetComponent<Collider2D>().gameObject.name == "TripletJump") {
			audioSource.Play ();
			maze.BellRung ();
		}

	}
}
