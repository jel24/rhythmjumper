using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beat : MonoBehaviour {

	private float speed;
	private Text text;
	private float distancePerBeat;
	private RectTransform rect;
	private int lifeTime;
	private int tempo;
	private int number;
	private bool miss;
	private bool hit;
	private float opacity;

	// Use this for initialization
	void Start () {
		speed = 0f;
		text = GetComponent<Text> ();
		rect = GetComponent<RectTransform> ();
		distancePerBeat = rect.rect.width;
		lifeTime = 0;
		miss = false;
		opacity = 0f;
	}

	void Update () {
		lifeTime++;
		if (!hit) {
			float value = -speed * Time.deltaTime / 60f;
			rect.Translate(value, 0f, 0f);

		}
		UpdateColor ();
	}

	public void InitializeBeat(int newTempo, int newNumber, Vector2 spawnPos){
		text.fontSize = 48;
		lifeTime = 0;
		this.transform.localPosition = spawnPos;
		tempo = newTempo;
		speed = newTempo / 60f * distancePerBeat; 
		//number = newNumber;
		text.text = "O";
		//text.text = number + "";
	}

	public void Miss(){
		miss = true;
		text.text = "X";
		text.color = new Color (1f, 0f, 0f, opacity);
	}

	public void ReturnJumps(){
		miss = false;
		hit = false;
		text.text = number + "";
		text.color = new Color (1f, 1f, 1f, opacity);

	}

	public void LastJump(){
		text.color = new Color (1f, .5f, 0f, opacity);
		miss = true;
	}

	public void Hit(){
		hit = true;
	}

	public void Reset(){
		text.fontSize = 48;
		lifeTime = 0;
		Color currentColor = text.color;
		opacity = 0f;
		text.color = new Color (currentColor.r, currentColor.g, currentColor.b, opacity);
		hit = false;
	}

	private void UpdateColor(){
		int framesPerBeat = 0;

		if (tempo != 0) {
			framesPerBeat = Mathf.RoundToInt (60 / (tempo / 60));
		}
			
		Color currentColor = text.color;

		if (hit) {
			text.fontSize += 3;
			opacity -= .10f;
		}

		if (!miss & !hit) {

			if (lifeTime < (tempo / 60f) * framesPerBeat) {
				opacity += .02f;

			} else {
				opacity -= .03f;			
			}
			
		} else if (miss) {

				opacity -= .03f;

		}

		text.color = new Color (currentColor.r, currentColor.g, currentColor.b, opacity);
	}

	// 300 units total in 4 beats
	// 75 units per beat
}
