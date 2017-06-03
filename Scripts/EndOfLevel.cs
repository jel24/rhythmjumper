using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour {

	public string targetLevel;

	private LevelManager levelManager;

	void Start(){
		levelManager = FindObjectOfType<LevelManager> ();
		if (!levelManager) {
			Debug.Log ("Can't find level manager.");
		}
	}

	void OnTriggerEnter2D(Collider2D c){
		if (c.tag == "Player") {
			levelManager.LoadLevel (targetLevel);
		}
	}
}
