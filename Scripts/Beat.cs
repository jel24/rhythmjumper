using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beat : MonoBehaviour {

	private float speed;
	private Text text;
	private float distancePerBeat;
	private RectTransform rect;
	private PlayerCounter parent;

	// Use this for initialization
	void Start () {
		speed = 0f;
		text = GetComponent<Text> ();
		rect = GetComponent<RectTransform> ();
		distancePerBeat = rect.rect.width;
		parent = GetComponentInParent<PlayerCounter> ();
	}

	void Update () {

		float value = -speed * Time.deltaTime / 60f + (parent.GetAdjustmentValue () / 60f);

		rect.Translate(value, 0f, 0f);

		//Color currentColor = text.color;
		//text.color = new Color (currentColor.r, currentColor.g, currentColor.b, currentColor.a - 0.04f);
	}

	public void InitializeBeat(int tempo, int number, Vector2 spawnPos){
		this.transform.localPosition = spawnPos;
		speed = tempo / 60f * distancePerBeat; 
		text.text = number + "";
	}

	public void Miss(){
//		text.text = "X";
//		text.color = Color.red;
	}

	public void LastJump(){
	//	text.text = "!!";
	//	text.colo	r = Color.red;
	}

	public void Reset(){
	//	counter = 0;
	}

	// 300 units total in 4 beats
	// 75 units per beat
}
