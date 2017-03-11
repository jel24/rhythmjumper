﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			if (coll.GetComponentInParent<PlayerController> () == null) {
				coll.GetComponent<PlayerController> ().ChangeWaterStatus (true);
				print ("In the water!");
			}

		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.GetComponentInParent<PlayerController> () == null) {
			coll.GetComponent<PlayerController> ().ChangeWaterStatus (false);
			print ("Out of the water.");
		}
	}

}