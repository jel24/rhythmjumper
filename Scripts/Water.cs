using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	bool waterSpam;

	void OnTriggerEnter2D(Collider2D coll)
	{

		if (coll.gameObject.tag == "Player") {
			coll.GetComponentInParent<PlayerController> ().ChangeWaterStatus (true);
			print ("In the water!");
			if (!waterSpam) {
				GetComponent<AudioSource> ().Play ();
				waterSpam = true;
				Invoke ("StopWaterSpam", 1f);
			}
		} 
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			coll.GetComponentInParent<PlayerController> ().ChangeWaterStatus (false);
			print ("Out of the water.");
		} 
	}

	private void StopWaterSpam(){
		waterSpam = false;
	}
}