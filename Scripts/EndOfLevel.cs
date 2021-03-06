﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour {

	public string targetLevel;
	public int fragmentsRequired;

	private int fragmentsOwned;
	private Text text;
	private LoadManager loader;
	private ProgressManager progManager;

	private bool loadTriggered;

	void Start(){

		loadTriggered = false;

		text = GetComponentInChildren<Text> ();
		if (!text) {
			Debug.Log ("Can't find exit text.");
		}

		loader = FindObjectOfType<LoadManager> ();
		if (!loader) {
			Debug.Log ("Can't find LoadManager.");
		}

		progManager = FindObjectOfType<ProgressManager> ();
		if (!progManager) {
			Debug.Log ("Unable to find progress manager.");

		}
	}

	void OnTriggerEnter2D(Collider2D c){
		//if (c.tag == "Player" && fragmentsOwned >= fragmentsRequired) {
		if (c.tag == "Player" && !loadTriggered) {
			loadTriggered = true;
			c.GetComponentInParent<PlayerStatusManager> ().ToggleActive ();
			loader.FadeOut ();
			Invoke ("LoadLevel", 1.25f);
			progManager.SetLatestLevel (targetLevel);
		}
	}

	private void LoadLevel(){
		SceneManager.LoadSceneAsync (targetLevel);

	}

	public void UpdateFragments(int number){
		fragmentsOwned = number;
		// text.text = number + " / " + fragmentsRequired + "";
	}
}
