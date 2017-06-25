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
		tempo = 0;
		beats = FindObjectsOfType<Beat> ();

		if (beats.Length != 4) {
			Debug.Log ("Cannot find 4 beats.");
		}
	}

	public void StartBeats(int t){
		tempo = t;
		for (int i = 0; i < beats.Length; i++) {
			beats [i].InitializeBeat (tempo, i, beatSpawnPosition);
		}
	}

	public void UpdateNumber(){

		counter++;
		if (counter > 4) {
			counter = 1;
		}
		beats [counter - 1].transform.localPosition = beatSpawnPosition;
		beats [counter - 1].Reset ();

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

	public void Hit(){
		int current = counter + 3;
		if (current > 4) {
			current -= 4;
		}
		Debug.Log (current);
		beats [current-1].Hit ();
	}
}
