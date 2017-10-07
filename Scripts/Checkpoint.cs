using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public float camSize;

	private Camera cam;
	private PowerupManager powerupManager;

	void Start(){
		cam = FindObjectOfType<Camera> ();
		if (!cam) {
			Debug.Log ("Unable to find camera!");
		}
		powerupManager = FindObjectOfType<PowerupManager> ();
		if (!powerupManager) {
			Debug.Log ("Unable to find powerup manager!");		
		}
	}

	void OnDrawGizmos(){
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			cam.GetComponent<CameraController>().SetDefaultSize (camSize);
			Debug.Log ("Setting default camera size on " + cam.name + " to " + camSize);
			coll.gameObject.GetComponentInParent<PlayerStatusManager> ().startLocation = new Vector3 (transform.position.x, transform.position.y, 0f);
			powerupManager.SaveState ();
		}
	}
}
