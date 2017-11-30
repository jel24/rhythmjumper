using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public Text[] menuButtons;

	private int menuState;
	private float cooldown;
	private ProgressManager progManager;

	void Start(){
		menuState = 0;
		UpdateMenuButtons ();

		progManager = FindObjectOfType<ProgressManager> ();
		if (!progManager) {
			Debug.Log ("Unable to find progress manager.");

		}
	}

	void Update(){
		float inputY = CrossPlatformInputManager.GetAxis ("Vertical");

		if (inputY > 0f && cooldown <= 0f) {
			menuState--;
			UpdateMenuButtons ();
		} else if (inputY < 0f && cooldown <= 0f) {
			menuState++;
			UpdateMenuButtons ();

		} else if (CrossPlatformInputManager.GetButtonDown("Jump")){
			TakeAction();
		}

		if (cooldown > 0f) {
			cooldown -= Time.deltaTime;
		}
	}

	private void UpdateText(Text t){
		if (menuButtons[menuState] == t){
			t.color = Color.green;
		} else {
			t.color = Color.white;
		}
	}

	private void UpdateMenuButtons(){
		cooldown += .2f;
		if (menuState >= menuButtons.Length) {
			menuState = 0;
		} else if (menuState < 0) {
			menuState = menuButtons.Length - 1;
		}

		foreach (Text t in menuButtons) {
			UpdateText (t);
		}
	}

	private void TakeAction(){
		switch (menuButtons[menuState].text) {
		case "Start":
			SceneManager.LoadSceneAsync ("level1-1");
			break;
		case "Continue":
			SceneManager.LoadSceneAsync (progManager.GetLatestLevel());
			break;
		case "Exit":
			Application.Quit();
			break;
		default:
			Application.Quit();
			break;
		}
	}
}
