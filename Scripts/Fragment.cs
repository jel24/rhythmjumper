using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour {

	public int fragmentNumber;
	private FragmentCounter counter;

	void Start(){
		counter = FindObjectOfType<FragmentCounter> ();
		if (!counter) {
			Debug.Log ("Unable to find Fragment Counter.");
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			counter.AddFragment (fragmentNumber);
		}
	}
}
