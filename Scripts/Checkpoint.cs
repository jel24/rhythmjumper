using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public float camSize;

	private FragmentCounter fragmentCounter;
	private Camera cam;

	void Start(){
		cam = FindObjectOfType<Camera> ();
		if (!cam) {
			Debug.Log ("Unable to find camera!");
		}
		fragmentCounter = FindObjectOfType<FragmentCounter> ();
		if (!fragmentCounter) {
			Debug.Log ("Unable to find fragment counter!");
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (transform.position, 1);
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			cam.GetComponent<CameraController>().SetDefaultSize (camSize);
			Debug.Log ("Setting default camera size on " + cam.name + " to " + camSize);
			coll.gameObject.GetComponentInParent<PlayerStatusManager> ().startLocation = new Vector3 (transform.position.x, transform.position.y, 0f);
			coll.gameObject.GetComponentInParent<PlayerStatusManager> ().SaveFragments (fragmentCounter.GetFragmentArray ());
		}
	}
}
