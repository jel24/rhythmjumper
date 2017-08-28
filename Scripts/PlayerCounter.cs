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
	private JumpTypeManager jumpTypeManager;

	// Use this for initialization
	void Start () {
		counter = 0;
		tempo = 0;
		streak = 0;
		beats = FindObjectsOfType<Beat> ();
		onBeat = false;
		if (beats.Length != 8) {
			Debug.Log ("Cannot find 8 beats.");
		}

		for (int i = 0; i < beats.Length; i++){
			beats[i].GetComponent<Text>().text = i + "";
		}

		jumpTypeManager = FindObjectOfType<JumpTypeManager> ();
		if (!jumpTypeManager) {
			Debug.Log ("Cannot find Jump Type Manager.");

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
		if (counter > 8) {
			counter = 1;
		}
		//print ("Updating beat " + counter + " " + Time.timeSinceLevelLoad);
		beats [counter - 1].transform.localPosition = beatSpawnPosition;
		beats [counter - 1].Reset ();

	}

	public void Miss(){
		int targetBeat = counter-4;
		print (targetBeat);
		if (targetBeat > 8) {
			targetBeat -= 8;
		} else if (targetBeat < 0) {
			targetBeat += 8;
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

	public void UpdateBeatDisplayForJumpType(JumpType newType){
		switch (newType) {
		case JumpType.Eighth:
			foreach (Beat b in beats) {
				b.UpdateForJumpType (true);
			}
			break;
		case JumpType.Quarter:
			for (int i = 0; i < beats.Length; i++) {
				if (i % 2 == 0) {
					beats[i].UpdateForJumpType (true);
				} else {
					beats[i].UpdateForJumpType (false);
				}
			}
			break;
		case JumpType.Half:
			for (int i = 0; i < beats.Length; i++) {
				if (i % 4 == 0) {
					beats [i].UpdateForJumpType (true);
				} else {
					beats [i].UpdateForJumpType (false);
				}
			}
			break;
		case JumpType.Whole:
				for (int i = 0; i < beats.Length; i++) {
					if (i % 8 == 0) {
						beats[i].UpdateForJumpType (true);
					} else {
						beats[i].UpdateForJumpType (false);
					}
				}
			break;
		}
	}

}
