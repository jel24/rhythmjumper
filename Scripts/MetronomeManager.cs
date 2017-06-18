using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MetronomeManager : MonoBehaviour {

	public int tempo;
	public Image[] powerUpImages;
	public float errorThreshold;
	public Image[] streakImages;
	public Image waterUI;

	public Vector3 spawnPoint; 

	private bool onBeat;
	private float bps;
	private bool reset;
	private int streak;
	private Color active;
	private Color inactive;
	private Color invisible;
	private Color partial;
	private PowerupManager powerupManager;
	private PlayerCounter playerCounter;
	private PlayerStatusManager statusManager;
	private MusicManager musicManager;

	// Use this for initialization
	void Start () {
		bps = tempo / 60f;

		statusManager = FindObjectOfType<PlayerStatusManager> ();
		if (!statusManager) {
			Debug.Log ("Unable to find player status manager!");
		}

		powerupManager = FindObjectOfType<PowerupManager> ();
		if (!powerupManager) {
			Debug.Log ("Unable to find powerup manager!");
		}

		musicManager = FindObjectOfType<MusicManager> ();
		if (!musicManager) {
			Debug.Log ("Unable to find music manager!");
		}
			
		playerCounter = FindObjectOfType<PlayerCounter> ();
		if (!playerCounter) {
			Debug.Log ("Unable to find player counter!");
		} else {
			playerCounter.SetTempo (tempo);
		}



		active = new Color(255f, 255f, 255f, 1f);
		inactive = new Color(255f, 255f, 255f, .25f);
		invisible = new Color(255f, 255f, 255f, 0f);
		partial = new Color(255f, 255f, 255f, .5f);
	}


	public bool IsOnBeat(){
		return musicManager.IsOnBeat ();
	}

	public void changePowerUpStatus (int type, bool onOff)
	{
		if (musicManager.musicLevel) {
			powerUpImages[type].gameObject.SetActive(onOff);
		}

	}

	public bool GetPowerUpStatus (int type){
		return powerUpImages [type].gameObject.activeInHierarchy;
	}

	public void Downbeat (){
		bps = tempo / 60f;
		if (!reset) {
			UpdatePlayerCounter ();
			Invoke ("Downbeat", 1f / bps);
		} else {
			playerCounter.StartBeats ();
		}
	}

	public void Reset(){
		reset = true;
		EndStreak ();
	}

	public void Restart(){
		reset = false;
		StartBeats ();
	}

	public void AddStreak(){
		if (musicManager.musicLevel) {
			streak++;

			if (streak == 1) {
				streakImages [0].color = active;
			} else if (streak == 2) {
				streakImages [1].color = active;
			} else if (streak == 3) {
				streakImages [2].color = active;
			} else if (streak == 4) {
				streakImages [3].color = active;
				//powerupManager.AddBuff("Streak");
			} else if (streak > 4) {

			}
		}
	}

	public void EndStreak(){
		streak = 0;
		playerCounter.Miss ();
		for (int i = 0; i < 4; i++) {
			streakImages [i].color = inactive;
			powerupManager.RemoveBuff ("Streak");
		}
	}

	public void ShowWaterUI(bool show){
		if (musicManager.musicLevel) {
			if (show) {
				waterUI.color = partial;
			} else {
				waterUI.color = invisible;
			}
		}


	}

	public int StreakStatus(){
		return streak;
	}

	public void LastJump(){
		if (musicManager.musicLevel) {
			playerCounter.LastJump ();
		}
	}

	private void UpdatePlayerCounter(){
		if (musicManager.musicLevel) {
			playerCounter.UpdateNumber ();
		}
	}

	public void ReturnJumps(){
		if (musicManager.musicLevel) {
			playerCounter.ReturnJumps ();
		}
	}

	public void StartBeats(){
		if (musicManager.musicLevel) {
			playerCounter.StartBeats ();
		}
	}
}
