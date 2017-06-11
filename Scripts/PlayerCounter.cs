using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCounter : MonoBehaviour {

	public Object beatPrefab;

	private Text text;
	private int counter;
	private int tempo;
	private Beat[] beats;
	private Transform parentTransform;

	// Use this for initialization
	void Start () {
		counter = 0;
		text = GetComponentInChildren<Text> ();
		if (!text) {
			Debug.Log ("Cannot find Player Counter text.");
		}
		parentTransform = GetComponentInParent<Transform> ();
		if (!parentTransform) {
			Debug.Log ("Cannot find Player transform.");
		}
		beats = FindObjectsOfType<Beat> ();
		if (beats.Length != 4) {
			Debug.Log ("Cannot find 4 beats.");
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetTempo(int t){
		tempo = t;
	}

	public void UpdateNumber(){
		Vector3 playerPosition = new Vector3 (100f, 50f, 0f);
		counter++;
		if (counter > 4) {
			counter = 1;
		}
		beats[counter-1].transform.position = playerPosition;
		beats[counter-1].InitializeBeat(tempo, counter);

	//	text.text = counter + "";
		text.color = Color.white;

	}

	public void Miss(){

	}

	public void LastJump(){

	}

	public void Reset(){

	}
		
}
