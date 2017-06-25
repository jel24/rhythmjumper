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
	public bool musicLevel;
	public Vector3 spawnPoint; 

	private bool onBeat;
	private float bps;
	private int streak;

	private PowerupManager powerupManager;
	private PlayerCounter playerCounter;
	private PlayerStatusManager statusManager;
	private AudioSource audioSource;
	private PlayerStatusManager playerStatusManager;

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
			
		playerStatusManager = FindObjectOfType<PlayerStatusManager> ();
		if (!playerStatusManager) {
			Debug.Log ("Unable to find player!");
		}

		playerCounter = FindObjectOfType<PlayerCounter> ();
		if (!playerCounter && musicLevel) {
			Debug.Log ("Unable to find player counter!");
		}
			
		Invoke ("Begin", 1f);

	}
		
	private void Begin(){
		audioSource = GetComponent<AudioSource>();
		audioSource.Play();
		playerStatusManager.StartLevel();

		if (musicLevel) {
			playerCounter.StartBeats (tempo);
			Downbeat ();
		}
	}

	public void changePowerUpStatus (int type, bool onOff)
	{
		powerUpImages[type].gameObject.SetActive(onOff);
	}

	public bool GetPowerUpStatus (int type){
		return powerUpImages [type].gameObject.activeInHierarchy;
	}

	public void Downbeat (){
		bps = tempo / 60f;
		UpdatePlayerCounter ();
		Invoke ("Downbeat", 1f / bps);
	}

	public void Reset(){
		EndStreak ();
	}

	public void AddStreak(){
		if (musicLevel) {
			playerCounter.Hit ();
			streak++;

			if (streak == 1) {
				//streakImages [0].color = active;
			} else if (streak == 2) {
				//streakImages [1].color = active;
			} else if (streak == 3) {
				//streakImages [2].color = active;
			} else if (streak == 4) {
				//streakImages [3].color = active;
				//powerupManager.AddBuff("Streak");
			} else if (streak > 4) {

			}
		}
	}

	public void EndStreak(){
		streak = 0;
		playerCounter.Miss ();
		for (int i = 0; i < 4; i++) {
			//streakImages [i].color = inactive;
			powerupManager.RemoveBuff ("Streak");
		}
	}

	public void ShowWaterUI(bool show){
		if (musicLevel) {
			if (show) {
				//waterUI.color = partial;
			} else {
				//waterUI.color = invisible;
			}
		}

	}

	public int StreakStatus(){
		return streak;
	}

	public void LastJump(){
		if (musicLevel) {
			playerCounter.LastJump ();
		}
	}

	private void UpdatePlayerCounter(){
		if (musicLevel) {
			playerCounter.UpdateNumber ();
		}
	}

	public void ReturnJumps(){
		if (musicLevel) {
			playerCounter.ReturnJumps ();
		}
	}
		
	public bool IsOnBeat(){

		if (!musicLevel) {
			return true;
		} else {

			float t = tempo * 1f;
			float time = audioSource.time;
			float threshold = errorThreshold;

			float beatDuration = 60f / t;

			if ((time % beatDuration <= threshold) || (time % beatDuration) >= (beatDuration - threshold)) {
				print ("On beat!");
				return true;
			} else {
				print ("Not on beat.");
				return false;
			}

		}
	}
}
