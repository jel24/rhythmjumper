using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfLevel : MonoBehaviour {

	public string targetLevel;
	public int fragmentsRequired;

	private int fragmentsOwned;
	private LevelManager levelManager;
	private Text text;

	void Start(){
		levelManager = FindObjectOfType<LevelManager> ();
		if (!levelManager) {
			Debug.Log ("Can't find level manager.");
		}
		text = GetComponentInChildren<Text> ();
		if (!text) {
			Debug.Log ("Can't find exit text.");
		}
	}

	void OnTriggerEnter2D(Collider2D c){
		if (c.tag == "Player" && fragmentsOwned >= fragmentsRequired) {
			levelManager.LoadLevel (targetLevel);
		}
	}

	public void UpdateFragments(int number){
		fragmentsOwned = number;
		text.text = number + " / " + fragmentsRequired + "";
	}
}
