using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

	public string PowerUpType;

	private PowerupManager powerupManager;

	void Start(){
		powerupManager = FindObjectOfType<PowerupManager> ();
		if (!powerupManager) {
			Debug.Log ("Unable to find powerup manager!");
		}
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			powerupManager.AddPowerUp (this);
		}
	}

	public string GetPowerUpType(){
		return PowerUpType;
	}
}
