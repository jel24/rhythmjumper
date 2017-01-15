using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MetronomeManager : MonoBehaviour {

	public float tempo;
	public Beat beatPrefab;
	public Image[] powerUpImages;

	private Vector3 spawnPoint; 
	private HashSet<Beat> beats;
	private Canvas canvas;
	private bool onBeat;
	private int ticker;
	private float bps;

	// Use this for initialization
	void Start () {
		beats = new HashSet<Beat>();
		spawnPoint = new Vector3(175f, 215f, 0f);
		canvas = this.transform.parent.GetComponent<Canvas>();
		bps = tempo / 60f;
		Debug.Log(bps);
	}
	
	// Update is called once per frame

	void Update ()
	{
		ticker++;

		if (ticker % Mathf.RoundToInt(60 / bps) == 0) {
			Beat newBeat = Instantiate(beatPrefab) as Beat; 
			newBeat.transform.SetParent(canvas.transform);
			newBeat.GetComponent<RectTransform>().anchoredPosition = spawnPoint;
			newBeat.InitiateBeat(400f / (4f / bps));


		}

	}

	public bool IsOnBeat(){
		return onBeat;
	}

	public void changePowerUpStatus (int type, bool onOff)
	{
		powerUpImages[type].gameObject.SetActive(onOff);
	}
}
