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

	private List<JumpTypeNote> typeList;
	private PlayerCounter counter;

	private Vector2[] jumpTypeNotePositions;

	// Use this for initialization
	void Start () {
		typeList = new List<JumpTypeNote> ();
		counter = FindObjectOfType<PlayerCounter> ();
		if (!counter) {
			Debug.Log ("Unable to find PlayerCounter");
		}
			
		jumpTypeNotePositions = new Vector2[7];

		jumpTypeNotePositions[0] = new Vector2 (-6f, 0f);
		jumpTypeNotePositions[1] = new Vector2 (-4f, 0f);
		jumpTypeNotePositions[2] = new Vector2 (-2f, 0f);
		jumpTypeNotePositions[3] = new Vector2 (0f, 0f);
		jumpTypeNotePositions[4] = new Vector2 (2f, 0f);
		jumpTypeNotePositions[5] = new Vector2 (4f, 0f);
		jumpTypeNotePositions[6] = new Vector2 (6f, 0f);


		AddJumpType (JumpType.Quarter);
		AddJumpType (JumpType.Half);
		AddJumpType (JumpType.Eighth);
		AddJumpType (JumpType.Whole);


	}

	public void AddJumpType(JumpType j){



		GameObject o = Instantiate (prefab, this.transform) as GameObject;

		o.GetComponent<JumpTypeNote> ().SetJumpType (j);
		typeList.Add (o.GetComponent<JumpTypeNote>());

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

		if (typeList.Count == 1) {
			SetCurrentType(j);
			counter.UpdateBeatDisplayForJumpType (j);
		}
	}

	public JumpType SwapLeft(){
		int newPos = findCurrentType () - 1;
		//print (newPos);

		if (newPos == -1) {
			newPos = typeList.Count - 1;
		}
			
		SetCurrentType(typeList [newPos].GetJumpType());
		return typeList [newPos].GetJumpType();
	}

	public JumpType SwapRight(){
		int newPos = findCurrentType () + 1;

		if (newPos >= typeList.Count) {
			newPos = 0;
		}
		//print (typeList[newPos]);

		SetCurrentType(typeList [newPos].GetJumpType());
		return typeList [newPos].GetJumpType();
	}

	private int findCurrentType(){
		int index = 0;
		for (int i = 0; i < typeList.Count; i++) {
			if (typeList [i].GetJumpType() == currentType) {
				index = i;
			}
		}
		return index;
	}

	private void SetCurrentType(JumpType newType){
		currentType = newType;
		counter.UpdateBeatDisplayForJumpType (newType);
		UpdateActiveDisplay ();

	}

	public JumpType getJumpType(){
		return currentType;
	}

	private void UpdateDisplayPositions(){

		List<JumpTypeNote> newList = new List<JumpTypeNote> ();

		foreach (JumpTypeNote t in typeList) {
			if (t.GetJumpType() == JumpType.Eighth) {
				newList.Add (t);
				break;
			}
		}
		foreach (JumpTypeNote t in typeList) {
			if (t.GetJumpType() == JumpType.Quarter) {
				newList.Add (t);
				break;
			}
		}
		foreach (JumpTypeNote t in typeList) {
			if (t.GetJumpType() == JumpType.Half) {
				newList.Add (t);
				break;
			}
		}
		foreach (JumpTypeNote t in typeList) {
			if (t.GetJumpType() == JumpType.Whole) {
				newList.Add (t);
				break;
			}
		}
	
		typeList = newList;

			switch (typeList.Count) {
			case 1: 
				typeList[0].GetComponent<Image>().rectTransform.anchoredPosition = jumpTypeNotePositions[3];
				break;
			case 2:
				typeList[0].GetComponent<Image>().rectTransform.anchoredPosition = jumpTypeNotePositions[2];
				typeList[1].GetComponent<Image>().rectTransform.anchoredPosition = jumpTypeNotePositions[4];
				break;
			case 3:
				typeList[0].GetComponent<Image>().rectTransform.anchoredPosition = jumpTypeNotePositions[1];
				typeList[1].GetComponent<Image>().rectTransform.anchoredPosition = jumpTypeNotePositions[3];
				typeList[2].GetComponent<Image>().rectTransform.anchoredPosition = jumpTypeNotePositions[5];
				break;
			case 4:		
				typeList[0].GetComponent<Image>().rectTransform.anchoredPosition = jumpTypeNotePositions[0];
				typeList[1].GetComponent<Image>().rectTransform.anchoredPosition = jumpTypeNotePositions[2];
				typeList[2].GetComponent<Image>().rectTransform.anchoredPosition = jumpTypeNotePositions[4];
				typeList[3].GetComponent<Image>().rectTransform.anchoredPosition = jumpTypeNotePositions[6];
				break;
			}

	}

	private void UpdateActiveDisplay(){

		int x = findCurrentType ();

		print (x);

		for (int i = 0; i < typeList.Count; i++) {
			typeList [i].GetComponent<Image>().color = Color.gray;
		}
			
		typeList [x].GetComponent<Image>().color = Color.white;

	}
}
