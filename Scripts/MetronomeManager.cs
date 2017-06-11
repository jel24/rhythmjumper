using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MetronomeManager : MonoBehaviour {

	public int tempo;
	public Image[] powerUpImages;
	public MusicManager musicManager;
	public float errorThreshold;
	public Image[] streakImages;
	public Image waterUI;

	public Vector3 spawnPoint; 

	private Canvas canvas;
	private bool onBeat;
	private float bps;
	private bool reset;
	private AudioSource chime;
	private int streak;
	private Color active;
	private Color inactive;
	private Color invisible;
	private Color partial;
	private PowerupManager powerupManager;
	private PlayerCounter playerCounter;
	private PlayerStatusManager statusManager;

	// Use this for initialization
	void Start () {
		canvas = this.transform.parent.GetComponent<Canvas>();
		bps = tempo / 60f;
		chime = GetComponent<AudioSource> ();

		statusManager = FindObjectOfType<PlayerStatusManager> ();
		if (!statusManager) {
			Debug.Log ("Unable to find player status manager!");
		}

		powerupManager = FindObjectOfType<PowerupManager> ();
		if (!powerupManager) {
			Debug.Log ("Unable to find powerup manager!");
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
		powerUpImages[type].gameObject.SetActive(onOff);
	}

	public bool GetPowerUpStatus (int type){
		return powerUpImages [type].gameObject.activeInHierarchy;
	}

	public void Downbeat (){
		bps = tempo / 60f;
		if (!reset) {
			UpdatePlayerCounter ();
			Invoke ("Downbeat", 1f / bps);
		}
	}

	public void Reset(){
		reset = true;
		playerCounter.Reset ();
		EndStreak ();
	}

	public void Restart(){
		reset = false;
	}

	public void AddStreak(){
		streak++;

		if (streak == 1) {
			streakImages [0].color = active;
		} else if (streak == 2) {
			streakImages [1].color = active;
		} else if (streak == 3) {
			streakImages [2].color = active;
		} else if (streak == 4) {
			streakImages [3].color = active;
			powerupManager.AddBuff("Streak");
		} else if (streak > 4) {

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
		if (show) {
			waterUI.color = partial;
		} else {
			waterUI.color = invisible;
		}

	}

	public int StreakStatus(){
		return streak;
	}

	public void LastJump(){
		playerCounter.LastJump ();
	}

	private void UpdatePlayerCounter(){
		playerCounter.UpdateNumber ();
	}
}
