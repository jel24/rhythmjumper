﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailManager : MonoBehaviour {

	public Object trailParticle;

	private int frameCounter;
	private List<Vector2> locations;
	private List<Object> particles;
	private bool recording;
	private Vector2 prevLoc;
	private Transform playerTransform;

	void Start (){
		locations = new List<Vector2> ();
		particles = new List<Object> ();
		recording = true;
		prevLoc = new Vector2 (0f, 0f);
		playerTransform = FindObjectOfType<PlayerController> ().gameObject.transform;
		if (!playerTransform) {
			Debug.Log ("Unable to find Player.");
		}
	}

	// Update is called once per frame
	void Update () {
		frameCounter++;
		if (frameCounter % 10 == 0 && recording){
			Vector2 loc = new Vector2 (playerTransform.position.x, playerTransform.position.y);
			
			if (loc != prevLoc){
				locations.Add (loc);
			}

			if (locations.Count > 50) {
				locations.RemoveRange (0, 1);
			}

			prevLoc = loc;
		}
	}


	public void StopTrail(){
		frameCounter = 0;
		foreach (Object o in particles){
			Destroy (o);
		}
		particles.Clear ();
		recording = false;
	}

	public void SpawnTrail(){
		foreach (Vector2 point in locations){
			particles.Add(Instantiate (trailParticle, point, Quaternion.identity, this.transform));
		}
		locations.Clear ();
		recording = true;
	}
		
}
