using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beat : MonoBehaviour {

	public float distancePerBeat;

	private float speed;
	private Text text;

	// Use this for initialization
	void Start () {
		speed = 0f;
		text = GetComponent<Text> ();
	}

	void Update () {
		gameObject.transform.Translate(-speed / 60f, 0f, 0f);
		//Color currentColor = text.color;
		//text.color = new Color (currentColor.r, currentColor.g, currentColor.b, currentColor.a - 0.04f);
	}

	// Update is called once per frame
	public void InitializeBeat(int tempo, int number){
		speed = (tempo / 60f) * distancePerBeat;
		text.text = number + "";
	}

	public void Miss(){
//		text.text = "X";
//		text.color = Color.red;
	}

	public void LastJump(){
	//	text.text = "!!";
	//	text.color = Color.red;
	}

	public void Reset(){
	//	counter = 0;
	}

	// 300 units total in 4 beats
	// 75 units per beat
}
