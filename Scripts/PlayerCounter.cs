using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCounter : MonoBehaviour {

	public Vector2 beatSpawnPosition;

	private Text text;
	private int counter;
	private int tempo;
	private Beat[] beats;
	private Vector2 playerPos;

	private bool onBeat;
	private Beat activeBeat;
	private int streak;

	// Use this for initialization
	void Start () {
		counter = 0;
		tempo = 0;
		streak = 0;
		beats = FindObjectsOfType<Beat> ();
		onBeat = false;
		if (beats.Length != 4) {
			Debug.Log ("Cannot find 4 beats.");
		}
	}

	void OnTriggerEnter2D (Collider2D coll){
		if (coll.GetComponent<Beat> ()) {
			//Debug.Log ("New beat.");
			activeBeat = coll.GetComponent<Beat> ();
			onBeat = true;
		}
	}

	void OnTriggerExit2D (Collider2D coll){
		if (coll.GetComponent<Beat> ()) {
			onBeat = false;
			coll.GetComponent<Beat>().StartFade ();
		}
	}

	public void StartBeats(int t){
		tempo = t;
		for (int i = 0; i < beats.Length; i++) {
			beats [i].InitializeBeat (tempo, i, beatSpawnPosition);
		}
	}

	public bool ActiveBeatHitBefore() {
		return activeBeat.BeatHitBefore ();
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
		int targetBeat = counter-2;
		print (targetBeat);
		if (targetBeat > 4) {
			targetBeat -= 4;
		} else if (targetBeat < 0) {
			targetBeat += 4;
		}
		beats [targetBeat].Miss ();
	}
		
	public void LastJump(){
		activeBeat.LastJump();
	}

	public void Hit(){
		activeBeat.Hit ();
		streak++;
	}

	public bool IsOnBeat(){
		return onBeat;
	}
		

	public void ResetStreak(){
		streak = 0;
	}

	public void EndStreak(){
		ResetStreak ();
		Miss ();
	}

	public int StreakStatus(){
		return streak;
	}

}
