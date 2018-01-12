using UnityEngine;
using System.Collections;

public enum Powerup{
	Phoenix,
	Fragment,
	Upgrade,
	TripletJump,
	QuarterJump
}

public enum PickupStatus{
	InLevel,
	Held,
	Used
}

public class Pickup : MonoBehaviour {

	public Powerup PowerUpType;
	private PickupStatus status;

	private PowerupManager powerupManager;

	void Start(){
		powerupManager = FindObjectOfType<PowerupManager> ();
		status = PickupStatus.InLevel;
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

	public Powerup GetPowerUpType(){
		return PowerUpType;
	}

	public PickupStatus GetStatus(){
		return status;
	}

	public void SetStatus(PickupStatus p){
		status = p;
		if (p == PickupStatus.InLevel) {
			gameObject.SetActive (true);
		} else if (p == PickupStatus.Held) {
			gameObject.SetActive (false);
		}
	}
}
