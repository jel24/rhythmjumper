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
	private HashSet<Buff> activeBuffs;

	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator> ();
		Powerup[] powerupArray = FindObjectsOfType<Powerup> ();
		powerups = new HashSet<Powerup>();
		foreach (Powerup p in powerupArray) {
			powerups.Add(p);
		}
		activeBuffs = new HashSet<Buff>();
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
			ResetBuffs ();
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
	}

	public void StartLevel ()
	{
		alive = true;
	}

	public void AddPowerUp (string powerUpType)
	{
		bool isBuffNew = true;
		foreach (Buff b in activeBuffs) {
			if (b.GetBuffType () == powerUpType) {
				isBuffNew = false;
			} 
		}
		if (isBuffNew) {
			Buff newBuff = ScriptableObject.CreateInstance<Buff> ();
			newBuff.SetBuffType (powerUpType);
			activeBuffs.Add (newBuff);

			Debug.Log(powerUpType);

			if (powerUpType == "Metronome") {
				metronomeManager.changePowerUpStatus(0, true);
			}

			if (powerUpType == "Streak") {
				metronomeManager.changePowerUpStatus(1, true);
			}

			if (powerUpType == "MetronomeActive") {
				metronomeManager.changePowerUpStatus(2, true);
			}

			if (powerUpType == "Grace") {
				metronomeManager.changePowerUpStatus(3, true);
			}
		}

	}

	public void RemovePowerUp (string powerUpType){
		Buff targetBuff = null;
		foreach (Buff b in activeBuffs) {
			if (b.GetBuffType () == powerUpType) {
				targetBuff = b;
			} 
		}

		if (targetBuff != null) {
			if (targetBuff.GetBuffType() == "Metronome") {
				metronomeManager.changePowerUpStatus(0, false);
			}

			if (targetBuff.GetBuffType() == "Streak") {
				metronomeManager.changePowerUpStatus(1, false);
			}

			if (targetBuff.GetBuffType() == "MetronomeActive") {
				metronomeManager.changePowerUpStatus(2, false);
			}

			if (targetBuff.GetBuffType() == "Grace") {
				metronomeManager.changePowerUpStatus(3, false);
			}
			activeBuffs.Remove (targetBuff);
		}
	}

	public bool HasBuff (string buff)
	{
		foreach (Buff b in activeBuffs) {
			if (b.GetBuffType () == buff) {
				return true;
			}
		}
		return false;
	}

	private void ResetBuffs(){
		metronomeManager.changePowerUpStatus(0, false);
		metronomeManager.changePowerUpStatus(1, false);
		metronomeManager.changePowerUpStatus(2, false);
		metronomeManager.changePowerUpStatus(3, false);
		activeBuffs.Clear ();
	}
}
