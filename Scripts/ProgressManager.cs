using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Upgrade
{
	TripletJump,
	EighthJump,
	QuarterJump,
	HalfJump,
	WholeJump
}

public class ProgressManager : MonoBehaviour {

	private HashSet<Upgrade> upgrades;


	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}

	// Use this for initialization
	void Start () {
		upgrades = new HashSet<Upgrade>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddUpgrade(Upgrade u){
		if (!upgrades.Contains(u)){
			upgrades.Add (u);
		}
	}

	public bool HasUpgrade(Upgrade u){
		return upgrades.Contains (u);
	}
}
