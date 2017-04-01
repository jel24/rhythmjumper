using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MetronomeManager : MonoBehaviour {

	public float tempo;
	public Beat beatPrefab;
	public Image[] powerUpImages;
	public MusicManager musicManager;
	public float errorThreshold;
	public Image[] streakImages;
	public Image waterUI;
	public PlayerStatusManager statusManager;
	public Vector3 spawnPoint; 

	private HashSet<Beat> beats;
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

	// Use this for initialization
	void Start () {
		beats = new HashSet<Beat>();
		canvas = this.transform.parent.GetComponent<Canvas>();
		bps = tempo / 60f;
		chime = GetComponent<AudioSource> ();
		//Debug.Log(bps);
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
		// Debug.Log (1f / bps);
		if (!reset) {
			Invoke ("Downbeat", 1f / bps);
//			Invoke ("thresholdOn", (1f / bps) - errorThreshold);
			Beat newBeat = Instantiate(beatPrefab) as Beat; 
			newBeat.transform.SetParent(canvas.transform);
			newBeat.GetComponent<RectTransform>().anchoredPosition = spawnPoint;
			newBeat.InitiateBeat(300f / (4f / bps));
//			Invoke ("thresholdOff", errorThreshold);
		}
		// chime.Play ();
	}

//	private void thresholdOn(){
//		onBeat = true;
//	}
//
//	private void thresholdOff(){
//		onBeat = false;
//	}

	public void Reset(){
		reset = true;
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
			statusManager.AddPowerUp("Streak");
		} else if (streak > 4) {

		}
	}

	public void EndStreak(){
		streak = 0;
		for (int i = 0; i < 4; i++) {
			streakImages [i].color = inactive;
			statusManager.RemovePowerUp ("Streak");
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
}
