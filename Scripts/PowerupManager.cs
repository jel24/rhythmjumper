using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour {

	public Text text;
	public Object progManagerPrefab;

	private Dictionary<Powerup, bool> powerUps;
	private Dictionary<Powerup, bool> savedState;

	private HashSet<string> buffs;
	private MetronomeManager metronomeManager;
	private EndOfLevel ending;
	private ProgressManager progManager;


	void Start(){
		ending = FindObjectOfType<EndOfLevel> ();
		if (!ending) {
			Debug.Log ("Unable to find ending!");
		}

		metronomeManager = FindObjectOfType<MetronomeManager> ();
		if (!metronomeManager) {
			Debug.Log ("Unable to find powerup manager!");
		}

		progManager = FindObjectOfType<ProgressManager> ();
		if (!progManager) {
			
			Instantiate (progManagerPrefab);
			progManager = FindObjectOfType<ProgressManager> ();
			Debug.Log ("Unable to find progress manager, creating one.");
			//FindObjectOfType<JumpTypeManager> ().AddJumpType (JumpType.Quarter);
			Invoke ("InitializeJumpTypesFromProgressManager", .25f);

		}

		buffs = new HashSet<string>();

		powerUps = new Dictionary<Powerup, bool>();

		foreach (Powerup p in FindObjectsOfType<Powerup>()) {
			if (p.GetPowerUpType() != "Permanent") {
				powerUps.Add (p, false);
			}
		}

		SaveState ();
	}


	private void InitializeJumpTypesFromProgressManager(){
		progManager.AddJumpType (JumpType.Quarter);
		FindObjectOfType<JumpTypeManager> ().AddJumpType (JumpType.Quarter);

	}

	// powerups are represented in the level

	public void AddPowerUp (Powerup p)
	{
		p.gameObject.SetActive (false);

		if (p.name.Contains ("Metronome")) {
			powerUps [p] = true;

			AddBuff ("Metronome");
		} else if (p.name.Contains ("Fragment")) {
			powerUps [p] = true;

			UpdateFragments ();
		} else if (p.name.Contains ("TripletJump")) {
			progManager.AddUpgrade (Upgrade.TripletJump);
		} else if (p.name.Contains ("QuarterJump")) {
			progManager.AddJumpType(JumpType.Quarter);
			FindObjectOfType<JumpTypeManager> ().AddJumpType (JumpType.Quarter);
			metronomeManager.musicLevel = true;
		}

	}

	// buffs are represented on the buff bar. powerups create buffs when held

	public void AddBuff(string buffType){
		buffs.Add (buffType);
		UpdateBuffs ();
	}

	public void RemoveBuff (string buffType){
		buffs.Remove (buffType);
		UpdateBuffs ();
	}

	private void EnableTripletJump(){

	}

	void UpdateBuffs ()
	{

		metronomeManager.changePowerUpStatus (0, buffs.Contains("Metronome"));
		metronomeManager.changePowerUpStatus (1, buffs.Contains("Streak"));
		metronomeManager.changePowerUpStatus (2, buffs.Contains("MetronomeActive"));
		metronomeManager.changePowerUpStatus (3, buffs.Contains("Grace"));

	}

	void UpdateFragments(){
		int count = 0;
		foreach (Powerup p in powerUps.Keys) {
			if (p.name.Contains("Fragment") && powerUps[p]){
				count++;
			}
		}
		text.text = count + "";
		ending.UpdateFragments (count);
	}

	public bool HasBuff (string b)
	{
		return buffs.Contains (b);
	}

	private void ResetBuffs(){
		buffs.Clear ();
		UpdateBuffs ();
	}

	public void SaveState(){
		savedState = new Dictionary<Powerup, bool> ();
		foreach (Powerup p in powerUps.Keys) {
			savedState.Add (p, powerUps [p]);
		}
	}

	public void LoadState(){
		ResetBuffs ();
		powerUps = new Dictionary<Powerup, bool> ();
		foreach (Powerup p in savedState.Keys) {
			powerUps.Add (p, savedState [p]);
			//Debug.Log ("Adding " + p.name + ", " + savedState[p] + " to saved PowerUps");

			if (p.GetPowerUpType() != "Upgrade") {
				p.gameObject.SetActive (!savedState[p]);

			}
			if (savedState [p]) {
				AddPowerUp (p);
			}
		}
		UpdateFragments ();
	}
}
