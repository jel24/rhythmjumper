using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCounter : MonoBehaviour {

	public Object beatPrefab;
	public Vector2 beatSpawnPosition;

	private Text text;
	private int counter;
	private int tempo;
	private Beat[] beats;
	private Vector2 playerPos;

	// Use this for initialization
	void Start () {
		counter = 0;
		/*text = GetComponentInChildren<Text> ();
		if (!text) {
			Debug.Log ("Cannot find Player Counter text.");
		}*/

		beats = FindObjectsOfType<Beat> ();
		if (beats.Length != 4) {
			Debug.Log ("Cannot find 4 beats.");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		//transform.position = player.transform.position;
	}

	public void StartBeats(){
		int count = 0;
		foreach (Beat b in beats){
			count++;
			b.InitializeBeat (tempo, count, beatSpawnPosition);
		}
	}

	public void SetTempo(int t){
		tempo = t;
	}

	public void UpdateNumber(){
		counter++;
		if (counter > 4) {
			counter = 1;
		}
		beats[counter-1].transform.localPosition = beatSpawnPosition;
		beats [counter - 1].Reset ();
	//	beats[counter-1].InitializeBeat(tempo, counter);

	//	text.text = counter + "";
	//	text.color = Color.white;

	}

	public void Miss(){
		foreach (Beat b in beats){
			b.Miss();
		}
	}

	public void ReturnJumps(){
		foreach (Beat b in beats){
			b.ReturnJumps();
		}
	}

	public void LastJump(){
		foreach (Beat b in beats){
			b.LastJump();
		}
	}

	public void Reset(){

	}
}
