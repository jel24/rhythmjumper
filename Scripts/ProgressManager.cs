﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Upgrade
{
	TripletJump,
}

public class ProgressManager : MonoBehaviour {

	[SerializeField]
	private HashSet<Upgrade> upgrades;

	[SerializeField]
	private HashSet<JumpType> jumpTypes;

	private string latestLevel;

	void Start() {
		DontDestroyOnLoad(gameObject);
		upgrades = new HashSet<Upgrade>();
		jumpTypes = new HashSet<JumpType>();
		string loadPrefs = PlayerPrefs.GetString ("LatestLevel");
		if (loadPrefs == "") {
			SetLatestLevel ("level1-1");
		} else {
			SetLatestLevel (loadPrefs);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void AddUpgrade(Upgrade u){
		if (!upgrades.Contains(u)){
			upgrades.Add (u);
		}
	}

	public void AddJumpType(JumpType j){
		if (!jumpTypes.Contains(j)){
			jumpTypes.Add (j);
		}
	}

	public void SetLatestLevel(string s){
		PlayerPrefs.SetString ("LatestLevel", s);
		SaveToPlayerPrefs ();
		latestLevel = s;
	}

	public string GetLatestLevel(){
		return latestLevel;
	}

	public bool HasUpgrade(Upgrade u){
		return upgrades.Contains (u);
	}

	public HashSet<JumpType> GetJumpTypes(){
		return jumpTypes;
	}

	public void SaveToPlayerPrefs(){
		if (jumpTypes.Contains(JumpType.Quarter)){
			PlayerPrefs.SetInt ("QuarterJump", 1);
		}
		if (upgrades.Contains(Upgrade.TripletJump)){
			PlayerPrefs.SetInt ("TripletJump", 1);
		}
	}

	public void LoadFromPlayerPrefs(){
		if (PlayerPrefs.GetInt("QuarterJump") == 1) {
			AddJumpType (JumpType.Quarter);
		}
		if (PlayerPrefs.GetInt("TripletJump") == 1) {
			AddUpgrade (Upgrade.TripletJump);
		}
	}
}
