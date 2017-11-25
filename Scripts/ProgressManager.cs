using System.Collections;
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

	void Start() {
		DontDestroyOnLoad(gameObject);
		upgrades = new HashSet<Upgrade>();
		jumpTypes = new HashSet<JumpType>();
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

	public bool HasUpgrade(Upgrade u){
		return upgrades.Contains (u);
	}

	public HashSet<JumpType> GetJumpTypes(){
		return jumpTypes;
	}
}
