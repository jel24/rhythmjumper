using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SceneManager {

	public void LoadLevel(string levelName){
		LoadSceneAsync (levelName);
	}

	public void Quit(){
		Application.Quit ();
	}
}
