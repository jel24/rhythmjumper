using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCounter : MonoBehaviour {

	private Text text;
	private int counter;


	// Use this for initialization
	void Start () {
		counter = 0;
		text = GetComponentInChildren<Text> ();
		if (!text) {
			Debug.Log ("Cannot find Player Counter text.");
		}
	}
	
	// Update is called once per frame
	void Update () {
		Color currentColor = text.color;
		text.color = new Color (currentColor.r, currentColor.g, currentColor.b, currentColor.a - 0.04f);
	}

	public void UpdateNumber(){
		counter++;
		if (counter > 4) {
			counter = 1;
		}
		text.text = counter + "";
		text.color = Color.white;

	}

	public void Miss(){
		text.text = "X";
		text.color = Color.red;
	}

	public void LastJump(){
		text.text = "!!";
		text.color = Color.red;
	}

	public void Reset(){
		counter = 0;
	}
}
