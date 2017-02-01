using UnityEngine;
using System.Collections;

public class Buff : ScriptableObject {

	private string buffType;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string GetBuffType(){
		return buffType;
	}

	public void SetBuffType (string type)
	{
		buffType = type;
		InitiateBuff();
	}

	private void InitiateBuff ()
	{

	}
}
