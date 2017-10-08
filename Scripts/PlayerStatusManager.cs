using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStatusManager : MonoBehaviour {

	public Vector3 startLocation;
	private Animator animator;
	private bool alive;
	private HashSet<Powerup> powerups;
	private PowerupManager powerupManager;
	private MetronomeManager metronomeManager;
	private TrailManager trailManager;
	private CameraController cameraController;


	void OnDrawGizmos(){
	}

	// Use this for initialization
	void Start ()
	{
		startLocation = transform.position;
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
	
		trailManager = FindObjectOfType<TrailManager> ();
		if (!trailManager) {
			Debug.Log ("Unable to find trail manager!");		
		}

		cameraController = FindObjectOfType<CameraController> ();
		if (!cameraController) {
			Debug.Log ("Unable to find camera controller!");		
		}

		alive = false;
	}
	
	public void Kill ()
	{
		//Debug.Log("told to kill");
		if (alive) {
			Debug.Log("Killed!");
			alive = false;
			Invoke("Respawn", 1f);
			animator.SetTrigger("death");
			if (metronomeManager.musicLevel) {
				//metronomeManager.Reset ();
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
		if (metronomeManager.musicLevel) {

			foreach (Powerup p in powerups) {
				p.gameObject.SetActive(true);
			}
			powerupManager.LoadState ();
		}

		transform.position = startLocation;
		animator.SetTrigger ("respawn");
		StartLevel ();

	}

	public void StartLevel ()
	{
		alive = true;
	}

	public void ToggleActive(){
		alive = !alive;
	}
		
}
