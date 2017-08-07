using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldLabel : MonoBehaviour {

	int randomOpacity;
	Text text;


	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		randomOpacity = Random.Range (0, 2);
		if (randomOpacity == 1) {
			text.color = new Color (text.color.r, text.color.g, text.color.b, text.color.a + 0.05f);
		} else {
			text.color = new Color (text.color.r, text.color.g, text.color.b, text.color.a - 0.05f);

		}
	}
}
