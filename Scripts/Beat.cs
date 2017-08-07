using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beat : MonoBehaviour {

	private float speed;
	private Text text;
	private float distancePerBeat;
	private RectTransform rect;
	private int tempo;
	//private int number;
	private bool miss;
	private bool hit;
	private bool fade;
	private float opacity;
	private int framesPerBeat;

	// Use this for initialization
	void Start () {
		speed = 0f;
		text = GetComponent<Text> ();
		rect = GetComponent<RectTransform> ();
		distancePerBeat = rect.rect.width;
		miss = false;
		opacity = 0f;
	}

	void Update () {
		float value = -speed * Time.deltaTime / 60f;
		rect.Translate (value, 0f, 0f);
		UpdateColor ();
	}

	public void InitializeBeat(int newTempo, int newNumber, Vector2 spawnPos){
		text.fontSize = 48;
		this.transform.localPosition = spawnPos;
		tempo = newTempo;
		speed = newTempo / 60f * distancePerBeat; 
		//number = newNumber;
		text.text = "O";
		//text.text = number + "";

		if (tempo != 0) {
			framesPerBeat = Mathf.RoundToInt (60 / (tempo / 60));
		}

	}

	public void Miss(){
		miss = true;
		text.text = "X";
		text.color = new Color (1f, 0f, 0f, opacity);
	}
		
	public void LastJump(){
		text.color = new Color (1f, .5f, 0f, opacity);
		text.text = "!";
		//miss = true;
	}

	public void Hit(){
		hit = true;
	}

	public bool BeatHitBefore(){
		return hit || miss;
	}

	public void StartFade(){
		fade = true;
	}

	public void Reset(){
		text.fontSize = 48;
		Color currentColor = text.color;
		opacity = 0f;
		text.color = new Color (1f, 1f, 1f, 0f);
		hit = false;
		miss = false;
		text.text = "O";
		fade = false;
	}

	private void UpdateColor(){

		Color currentColor = text.color;

		if (hit) {
			text.fontSize += 3;
			opacity -= .10f;
		} else if (fade) {
			opacity -= .03f;
		} else {
			opacity += .03f;

		}

		text.color = new Color (currentColor.r, currentColor.g, currentColor.b, opacity);
	}

	// 300 units total in 4 beats
	// 75 units per beat
}
