using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour {

	public Text text;
	private Dictionary<Powerup, bool> powerUps;
	private Dictionary<Powerup, bool> savedState;

	private HashSet<string> buffs;
	private MetronomeManager metronomeManager;
	private EndOfLevel ending;

	void Start(){
		ending = FindObjectOfType<EndOfLevel> ();
		if (!ending) {
			Debug.Log ("Unable to find ending!");
		}

		metronomeManager = FindObjectOfType<MetronomeManager> ();
		if (!metronomeManager) {
			Debug.Log ("Unable to find powerup manager!");
		}

		buffs = new HashSet<string>();

		powerUps = new Dictionary<Powerup, bool>();

		foreach (Powerup p in FindObjectsOfType<Powerup>()) {
			powerUps.Add (p, false);
		}

		SaveState ();
	}

	// powerups are represented in the level

	public void AddPowerUp (Powerup p)
	{
		powerUps [p] = true;
		p.gameObject.SetActive (false);

		if (p.name.Contains("Metronome")) {
			AddBuff ("Metronome");
		} else if (p.name.Contains("Fragment")) {
			UpdateFragments ();
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
			Debug.Log ("Adding " + p.name + ", " + savedState[p] + " to saved PowerUps");
			p.gameObject.SetActive (!savedState[p]);
			if (savedState [p]) {
				AddPowerUp (p);
			}
		}
		UpdateFragments ();
	}
}
