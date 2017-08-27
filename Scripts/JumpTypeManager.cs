using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum JumpType {
	Eighth,
	Quarter,
	Half,
	Whole
}


public class JumpTypeManager : MonoBehaviour {

	private JumpType currentType;
	public Sprite[] graphics;
	public Object prefab;

	private List<JumpType> typeList;
	private PlayerCounter counter;

	// Use this for initialization
	void Start () {
		typeList = new List<JumpType> ();
		counter = FindObjectOfType<PlayerCounter> ();
		if (!counter) {
			Debug.Log ("Unable to find PlayerCounter");
		}


		AddJumpType (JumpType.Eighth);
		AddJumpType (JumpType.Quarter);
		AddJumpType (JumpType.Half);

		AddJumpType (JumpType.Whole);



	}

	public void AddJumpType(JumpType j){

		if (typeList.Count == 0) {
			SetCurrentType(j);
			counter.UpdateBeatDisplayForJumpType (j);
		}

		typeList.Add (j);
		GameObject o = Instantiate (prefab, this.transform) as GameObject;

		print (o.name);

		switch (j) {
		case JumpType.Eighth: 
			o.GetComponent<Image>().sprite = graphics[0];
			break;
		case JumpType.Quarter:
			o.GetComponent<Image>().sprite = graphics[1];
			break;
		case JumpType.Half:
			o.GetComponent<Image>().sprite = graphics[2];
			break;
		case JumpType.Whole:			
			o.GetComponent<Image>().sprite = graphics[3];
			break;
		}

		UpdateDisplayPositions ();
	}

	public JumpType SwapLeft(){
		int newPos = findCurrentType () - 1;
		print (newPos);

		if (newPos == -1) {
			newPos = typeList.Count - 1;
		}
			
		print (typeList[newPos]);
		SetCurrentType(typeList [newPos]);
		return typeList [newPos];
	}

	public JumpType SwapRight(){
		int newPos = findCurrentType () + 1;

		if (newPos >= typeList.Count) {
			newPos = 0;
		}
		print (typeList[newPos]);

		SetCurrentType(typeList [newPos]);
		return typeList [newPos];
	}

	private int findCurrentType(){
		int index = 0;
		for (int i = 0; i < typeList.Count; i++) {
			if (typeList [i] == currentType) {
				index = i;
			}
		}
		return index;
	}

	private void SetCurrentType(JumpType newType){
		currentType = newType;
		counter.UpdateBeatDisplayForJumpType (newType);

	}

	public JumpType getJumpType(){
		return currentType;
	}

	private void UpdateDisplayPositions(){

		Image[] children = GetComponentsInChildren<Image> ();

		switch (typeList.Count) {
		case 1: 
			children [1].rectTransform.anchoredPosition = new Vector2 (0f, 0f);
			break;
		case 2:
			children [1].rectTransform.anchoredPosition = new Vector2 (-2f, 0f);
			children [2].rectTransform.anchoredPosition = new Vector2 (2f, 0f);
			break;
		case 3:
			children [1].rectTransform.anchoredPosition = new Vector2 (-4f, 0f);
			children [2].rectTransform.anchoredPosition = new Vector2 (0f, 0f);
			children [3].rectTransform.anchoredPosition = new Vector2 (4f, 0f);
			break;
		case 4:		
			children [1].rectTransform.anchoredPosition = new Vector2 (-6f, 0f);
			children [2].rectTransform.anchoredPosition = new Vector2 (-2f, 0f);
			children [3].rectTransform.anchoredPosition = new Vector2 (2f, 0f);
			children [4].rectTransform.anchoredPosition = new Vector2 (6f, 0f);
			break;
		}
	}
}
