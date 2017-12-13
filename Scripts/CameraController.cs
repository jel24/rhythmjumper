using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject background;
	public float thresholdX;
	public float thresholdY;
	public float targetSize;
	public float defaultSize;

	private Camera cam;
	private Transform playerTransform;

	void Start () {
		cam = GetComponent<Camera> ();
		playerTransform = FindObjectOfType<PlayerController> ().gameObject.transform;
		if (!playerTransform) {
			Debug.Log ("Unable to find Player.");
		}
	}

	// Update is called once per frame
	void LateUpdate ()
	{
		float transformX = 0f;
		float transformY = 0f;
		Vector3 playerPos = playerTransform.position;
		Vector3 camPos = transform.position;

		if (Mathf.Abs (playerPos.x - camPos.x) > thresholdX) {
			transformX = (playerPos.x - camPos.x) * Time.deltaTime;
		}

		if (Mathf.Abs (playerPos.y - camPos.y) > thresholdY) {
			transformY = (playerPos.y - camPos.y) * Time.deltaTime * 2;
		} 

		if (transformX != 0f || transformY != 0f) {
			Vector3 translationVector = new Vector3(transformX, transformY, 0f);
			transform.Translate(translationVector);
			background.transform.Translate(-translationVector/15f);
		}

		if (cam.orthographicSize > targetSize) {
			cam.orthographicSize = cam.orthographicSize - .01f;
		} else if (cam.orthographicSize < targetSize) {
			cam.orthographicSize = cam.orthographicSize + .01f;
		}

	}

	public void ChangeCameraSize(float f){
		targetSize = f;
	}

	public void SetDefaultSize(float f){
		defaultSize = f;
	}

	public void SetSizeToDefault(){
		ChangeCameraSize (defaultSize);
	}
}
