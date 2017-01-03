using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MetronomeManager : MonoBehaviour {

	public Image[] beats;
	public float tempo;


	private int ticker;
	private float timeTracker;

	// Use this for initialization
	void Start () {
		ticker = 0;
		UpdatePosition();
	}
	
	// Update is called once per frame

	void UpdatePosition ()
	{
		for (int i = 0; i < 4; i++) {

			beats [i].transform.Translate (new Vector3 (-100 / 60f, 0f, 0f));

		}

		if (ticker == 0) {
			beats [0].GetComponent<RectTransform>().anchoredPosition = new Vector3(175f, 215f, 0f);
			timeTracker = Time.time;
		} else if (ticker == 60) {
			beats [1].GetComponent<RectTransform>().anchoredPosition = new Vector3(175f, 215f, 0f);
			//Debug.Log(Time.time - timeTracker + " since last beat update.");
			timeTracker = Time.time;
		} else if (ticker == 120) {
			beats [2].GetComponent<RectTransform>().anchoredPosition = new Vector3(175f, 215f, 0f);
			//Debug.Log(Time.time - timeTracker + " since last beat update.");
			timeTracker = Time.time;
		} else if (ticker == 180) {
			beats [3].GetComponent<RectTransform>().anchoredPosition = new Vector3(175f, 215f, 0f);
			//Debug.Log(Time.time - timeTracker + " since last beat update.");
			timeTracker = Time.time;
		} else if (ticker == 239) {
			ticker = -1;
		}

		ticker++;
		Invoke("UpdatePosition", 1/tempo);
	}

	public bool IsOnBeat(){
		return (ticker > 55 && ticker < 65) ||
			   (ticker > 115 && ticker < 125) ||
			   (ticker > 175 && ticker < 185) ||
			   (ticker > 235 && ticker < 5);
	}
}
