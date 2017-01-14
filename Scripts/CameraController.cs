using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public GameObject background;
	public float thresholdX;
	public float thresholdY;
	
	// Update is called once per frame
	void Update ()
	{
		float transformX = 0f;
		float transformY = 0f;
		Vector3 playerPos = player.transform.position;
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
			background.transform.Translate(translationVector/5f);
		}


	}
}
