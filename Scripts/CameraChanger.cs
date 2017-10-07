using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour {

	public float camSize;

	private CameraController cameraController;

	void Start() {
		cameraController = FindObjectOfType<CameraController> ();
	}

	void OnDrawGizmos(){
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			cameraController.ChangeCameraSize (camSize);
		}
	}
}
