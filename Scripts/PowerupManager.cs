using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour {

	public Text text;
	public Object progManagerPrefab;

	private HashSet<Pickup> powerUps;
	private Dictionary<Pickup, PickupStatus> savedState;

	private MetronomeManager metronomeManager;
	private EndOfLevel ending;
	private ProgressManager progManager;
	private PowerupUIManager powerupUI;

	void Start(){
		ending = FindObjectOfType<EndOfLevel> ();
		if (!ending) {
			Debug.Log ("Unable to find ending!");
		}

		metronomeManager = FindObjectOfType<MetronomeManager> ();
		if (!metronomeManager) {
			Debug.Log ("Unable to find powerup manager!");
		}

		powerupUI = FindObjectOfType<PowerupUIManager> ();
		if (!powerupUI) {
			Debug.Log ("Unable to find powerup UI!");
		}

		progManager = FindObjectOfType<ProgressManager> ();
		if (!progManager) {
			
			Instantiate (progManagerPrefab);
			progManager = FindObjectOfType<ProgressManager> ();
			Debug.Log ("Unable to find progress manager, creating one.");
			//FindObjectOfType<JumpTypeManager> ().AddJumpType (JumpType.Quarter);
			Invoke ("InitializeJumpTypesFromProgressManager", .25f);

		}
			
		powerUps = new HashSet<Pickup>();

		GameObject.Find ("PowerupImage_Metronome");

		foreach (Pickup p in FindObjectsOfType<Pickup>()) {
			if (p.GetPowerUpType() != Powerup.Upgrade) {
				powerUps.Add (p);
				print (p.GetPowerUpType());
			}
		}

		powerupUI.InitiatePowerupUI (powerUps);

		SaveState ();
	}


	private void InitializeJumpTypesFromProgressManager(){
		progManager.AddJumpType (JumpType.Quarter);
		FindObjectOfType<JumpTypeManager> ().AddJumpType (JumpType.Quarter);

	}

	// powerups are represented in the level

	public void AddPowerUp (Pickup p)
	{
		print ("Adding powerup " + p.GetPowerUpType ());
		p.SetStatus (PickupStatus.Held);

		switch (p.GetPowerUpType ()) {

		case Powerup.Phoenix:
			break;
		case Powerup.TripletJump:
			progManager.AddUpgrade (Upgrade.TripletJump);
			break;
		case Powerup.Fragment:
			//UpdateFragments ();
			break;
		case Powerup.QuarterJump:
			progManager.AddJumpType(JumpType.Quarter);
			FindObjectOfType<JumpTypeManager> ().AddJumpType (JumpType.Quarter);
			metronomeManager.musicLevel = true;
			break;
		default:
			break;
		}

		powerupUI.UpdateUI (powerUps);
	}

	void UpdateFragments(){
		int count = 0;
		//foreach (Powerup p in powerUps.Keys) {
		//	if (p.name.Contains("Fragment") && powerUps[p]){
		//		count++;
		//	}
		//}
		//text.text = count + "";
		//ending.UpdateFragments (count);
	}
		

	public void SaveState(){
		savedState = new Dictionary<Pickup, PickupStatus> ();
		foreach (Pickup p in powerUps) {
			savedState.Add (p, p.GetStatus());
		}
	}

	public void LoadState(){
		powerUps = new HashSet<Pickup> ();
		foreach (Pickup p in savedState.Keys) {
			powerUps.Add (p);
			p.SetStatus(savedState [p]);
		}
		powerupUI.UpdateUI (powerUps);
		UpdateFragments ();
	}

	public bool HasPowerup(Powerup p){
		foreach (Pickup q in powerUps){
			if (q.GetPowerUpType () == p && q.GetStatus() == PickupStatus.Held) {
				return true;
			}
		}
		return false;
	}

	public void UsePowerup(Powerup p){
		Pickup targetPickup = null;
		foreach (Pickup q in powerUps){
			if (q.GetPowerUpType () == p && q.GetStatus() == PickupStatus.Held) {
				targetPickup = q;
			}
		}
		if (targetPickup) {
			targetPickup.SetStatus(PickupStatus.Used);
			powerupUI.UpdateUI (powerUps);
		}
	}
}
