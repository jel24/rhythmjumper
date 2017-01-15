﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStatusManager : MonoBehaviour {


	public Vector3 startLocation;
	public MusicManager musicManager;
	public MetronomeManager metronomeManager;

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
		Debug.Log("told to kill");
		if (alive) {
			Debug.Log("killed!");
			alive = false;
			Invoke("Respawn", 2f);
			animator.SetTrigger("death");
			musicManager.GetComponent<AudioSource>().Stop();
		}

	}

	public bool IsAlive ()
	{
		return alive;
	}

	private void Respawn ()
	{
		this.transform.position = startLocation;
		animator.SetTrigger ("respawn");
		musicManager.StartMusic ();
		foreach (Powerup p in powerups) {
			p.gameObject.SetActive(true);
		}
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
			Buff newBuff = new Buff ();
			newBuff.SetBuffType (powerUpType);
			activeBuffs.Add (newBuff);

			Debug.Log(powerUpType);

			if (powerUpType == "Metronome") {
				metronomeManager.changePowerUpStatus(0, true);
			}


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
}
