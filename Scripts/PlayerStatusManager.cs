using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStatusManager : MonoBehaviour {


	public Vector3 startLocation;
	public MusicManager musicManager;
	public MetronomeManager metronomeManager;
	public CameraController cameraController;
	public TrailManager trailManager;

	private Animator animator;
	private bool alive;
	private HashSet<Powerup> powerups;
	private bool[] fragments;
	private FragmentCounter counter;
	private PowerupManager powerupManager;

	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator> ();
		powerups = new HashSet<Powerup>();

		counter = FindObjectOfType<FragmentCounter> ();
		if (!counter) {
			Debug.Log ("Unable to find Fragment Counter.");
		}

		powerupManager = FindObjectOfType<PowerupManager> ();
		if (!powerupManager) {
			Debug.Log ("Unable to find powerup manager!");		
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
			musicManager.GetComponent<AudioSource>().Stop();
			metronomeManager.Reset ();
			cameraController.SetSizeToDefault ();
			trailManager.StopTrail ();
			trailManager.SpawnTrail ();
		}

	}

	public bool IsAlive ()
	{
		return alive;
	}

	private void Respawn ()
	{
		transform.position = startLocation;
		animator.SetTrigger ("respawn");
		musicManager.StartMusic ();
		foreach (Powerup p in powerups) {
			p.gameObject.SetActive(true);
		}
		metronomeManager.Restart();
		powerupManager.LoadState ();
	}

	public void StartLevel ()
	{
		alive = true;
	}
		
}
