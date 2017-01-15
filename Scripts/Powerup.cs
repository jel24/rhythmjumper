using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

	public string PowerUpType;

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			coll.GetComponentInParent<PlayerStatusManager>().AddPowerUp(PowerUpType);
			gameObject.SetActive(false);
		}
	}
}
