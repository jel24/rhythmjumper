using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupUIManager : MonoBehaviour {

	public Object phoenixImage;
	public Vector3[] imageLocation;

	private Dictionary<Powerup, GameObject> powerupImages;

	void Awake(){
		powerupImages = new Dictionary<Powerup, GameObject> ();
	}

	// Use this for initialization
	public void InitiatePowerupUI(HashSet<Pickup> powerups) {
		foreach (Pickup p in powerups) {
			switch (p.GetPowerUpType()) {
				case Powerup.Phoenix:
					GameObject newImage = Instantiate (phoenixImage, imageLocation[0], Quaternion.identity, gameObject.transform) as GameObject;
					newImage.SetActive (false);
					powerupImages.Add (Powerup.Phoenix, newImage);
					print ("Adding Phoenix");
					break;
				default:
					break;
			}
		}
	}


	public void UpdateUI(HashSet<Pickup> powerups){
		print ("Updating powerup UI");
		foreach (Pickup p in powerups) {
			if (p.GetPowerUpType () != Powerup.Fragment) {
				print (p.GetPowerUpType ());
				if (p.GetStatus() == PickupStatus.Held) {
					powerupImages [p.GetPowerUpType ()].SetActive (true);
				} else {
					powerupImages [p.GetPowerUpType ()].SetActive (false);
				}
			}

		}
	}
}
