using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour {

	public string targetLevel;
	public LevelManager levelManager;

	void OnTriggerEnter2D(Collider2D c){
		if (c.tag == "Player") {
			levelManager.LoadLevel (targetLevel);
		}
	}
}
