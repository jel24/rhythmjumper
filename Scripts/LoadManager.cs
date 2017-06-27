using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour {

	private Image panel;
	private Image loader;
	private bool fadeIn;
	private bool fadeOut;

	// Use this for initialization
	void Start () {
		panel = GetComponent<Image> ();
		loader = GetComponentsInChildren<Image> () [1];
		if (!loader) {
			Debug.Log ("LoadManager can't find image in child.");
		}
		Debug.Log (loader.name);
		fadeIn = true;
	}

	// Update is called once per frame
	void Update () {
		if (fadeIn) {
			Color c = panel.color;
			Color l = loader.color;
			if (c.a < 0f) {
				fadeIn = false;
			} else {
				loader.color = new Color (l.r, l.g, l.b, l.a - 0.05f);
				panel.color = new Color (c.r, c.g, c.b, c.a - 0.05f);
			}
		} else if (fadeOut) {
			Color c = panel.color;
			Color l = loader.color;
			if (c.a > 1f) {
				fadeOut = false;
			} else {
				loader.color = new Color (l.r, l.g, l.b, l.a + 0.05f);
				panel.color = new Color (c.r, c.g, c.b, c.a + 0.05f);
			}
		}
	}

	public void FadeOut () {
		fadeOut = true;
	}
}
