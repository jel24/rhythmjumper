using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FragmentCounter : MonoBehaviour {

	public Fragment[] fragments;

	private Text text;
	private bool[] fragmentFlags;
	private int count;

	void Start (){
		text = GetComponentInChildren<Text> ();
		fragmentFlags = new bool[5];
		count = 0;
	}
		
	public void AddFragment(int fragmentNumber){
		fragmentFlags [fragmentNumber] = true;
		Debug.Log ("Acquired fragment " + fragmentNumber);
		UpdateFragment (fragmentNumber);
		text.text = Count () + "";
	}

	public bool[] GetFragmentArray(){
		return fragmentFlags;
	}

	public void Reset(bool[] newFlags){
		fragmentFlags = newFlags;
		for (int i = 0; i < 5; i++) {
			UpdateFragment (i);
		}
		text.text = Count () + "";
	}

	private int Count(){
		count = 0;
		foreach (bool fragment in fragmentFlags) {
			if (fragment) {
				count++;
			}
		}
		return count;
	}

	private void UpdateFragment(int fragmentNumber){
		Debug.Log ("Updating fragment " + fragmentNumber);
		Debug.Log (fragmentFlags[fragmentNumber]);
		if (fragmentFlags [fragmentNumber]) {
			Debug.Log ("Fragment " + fragmentNumber + " is now disabled.");
			fragments [fragmentNumber].enabled = false;
		} else {
			Debug.Log ("Fragment " + fragmentNumber + " is now enabled.");
			fragments [fragmentNumber].enabled = true;
		}
	}
}
