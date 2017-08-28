using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MetronomeManager : MonoBehaviour {

	public int tempo;
	public Image[] powerUpImages;
	public float errorThreshold;
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
			
		Invoke ("Begin", .5f);

	}
		
	private void Begin(){
		audioSource = GetComponent<AudioSource>();
		audioSource.Play();
		playerStatusManager.StartLevel();

		if (musicLevel) {
			playerCounter.StartBeats (tempo);
			bps = tempo / 60f;
			InvokeRepeating ("Downbeat", 1f / bps, 1f / bps);
			InvokeRepeating ("Upbeat", 1f / bps / 2, 1f / bps);
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
		UpdatePlayerCounter ();
	}

	public void Upbeat () {
		UpdatePlayerCounter ();
	}
		
	private void UpdatePlayerCounter(){
		if (musicLevel) {
			playerCounter.UpdateNumber ();
		}
	}
}
