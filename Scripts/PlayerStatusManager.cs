using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStatusManager : MonoBehaviour {


	public Vector3 startLocation;

	public CameraController cameraController;
	public TrailManager trailManager;

	private Animator animator;
	private bool alive;
	private HashSet<Powerup> powerups;
	private bool[] fragments;
	private PowerupManager powerupManager;
	private MetronomeManager metronomeManager;
	private MusicManager musicManager;

	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator> ();
		powerups = new HashSet<Powerup>();

		powerupManager = FindObjectOfType<PowerupManager> ();
		if (!powerupManager) {
			Debug.Log ("Unable to find powerup manager!");		
		}

		metronomeManager = FindObjectOfType<MetronomeManager> ();
		if (!metronomeManager) {
			Debug.Log ("Unable to find metronome manager!");		
		}

		musicManager = FindObjectOfType<MusicManager> ();
		if (!musicManager) {
			Debug.Log ("Unable to find music manager!");
		}


		fragments = new bool[5] { false, false, false, false, false };

	}
	
	public void Kill ()
	{
		//Debug.Log("told to kill");
		if (alive) {
			Debug.Log("killed!");
			alive = false;
			Invoke("Respawn", 1f);
			animator.SetTrigger("death");
			if (musicManager.musicLevel) {
				musicManager.GetComponent<AudioSource>().Stop();
				metronomeManager.Reset ();
				trailManager.StopTrail ();
				trailManager.SpawnTrail ();
			}
			cameraController.SetSizeToDefault ();
		}

	}

	public bool IsAlive ()
	{
		return alive;
	}

	private void Respawn ()
	{
		if (musicManager.musicLevel) {

			foreach (Powerup p in powerups) {
				p.gameObject.SetActive(true);
			}
			metronomeManager.Restart();
			powerupManager.LoadState ();
		}

		musicManager.StartMusic ();
		transform.position = startLocation;
		animator.SetTrigger ("respawn");

	}

	public void StartLevel ()
	{
		alive = true;
	}
		
}
