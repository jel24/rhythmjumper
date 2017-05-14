using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour {

	private Text text;
	private Dictionary<Powerup, bool> powerUps;
	private Dictionary<Powerup, bool> savedState;

	private HashSet<string> buffs;
	private MetronomeManager metronomeManager;

	void Start(){
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
		powerUps = new Dictionary<Powerup, bool> ();
		foreach (Powerup p in savedState.Keys) {
			powerUps.Add (p, powerUps [p]);
			p.gameObject.SetActive (!savedState[p]);
		}
	}
}
