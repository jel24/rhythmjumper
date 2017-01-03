using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour {

	public Text countDownText;
	public GameObject player;

	private AudioSource audioSource;
	private int timeToStart;

	void Start ()
	{
		StartMusic();
		audioSource = GetComponent<AudioSource>();
	}

	public void StartMusic ()
	{
		timeToStart = 3;
		CountDown();
	}

	private void CountDown ()
	{
		if (timeToStart > 0) {
			countDownText.text = timeToStart.ToString ();
			timeToStart--;
			Invoke ("CountDown", 1f);
		} else if (timeToStart == 0) {
			countDownText.text = "Go!";
			timeToStart--;
			Invoke ("CountDown", 1f);
			audioSource.Play();
			player.GetComponent<PlayerStatusManager>().StartLevel();

		} else if (timeToStart < 0) {
			countDownText.text = "";
			timeToStart = 3;
		}

	}
}
