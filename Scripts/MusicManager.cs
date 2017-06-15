using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour {

	public Text countDownText;
	public GameObject player;
	public bool musicLevel;

	private MetronomeManager metronome;
	private AudioSource audioSource;
	private int timeToStart;

	void Start ()
	{
		metronome = FindObjectOfType<MetronomeManager> ();
		if (!metronome) {
			Debug.Log ("MusicManager can't find MetronomeManager.");
		}
			
		audioSource = GetComponent<AudioSource>();

		if (musicLevel) {
			StartMusic();
		} else {
			audioSource.Play();
			player.GetComponent<PlayerStatusManager>().StartLevel();
		}
	}

	public void StartMusic ()
	{
		timeToStart = 3;
		CountDown();
	}

	private void CountDown ()
	{

		if (timeToStart > 0 && musicLevel) {
			countDownText.text = timeToStart.ToString ();
			timeToStart--;
			Invoke ("CountDown", 1f);
		} else if (timeToStart == 0 || !musicLevel) {
			if (musicLevel) {
				countDownText.text = "Go!";
				audioSource.Play();
				metronome.Downbeat ();
				Invoke ("CountDown", 1f);
			}
			timeToStart--;
			player.GetComponent<PlayerStatusManager>().StartLevel();
		} else if (timeToStart < 0) {
			countDownText.text = "";
			timeToStart = 3;
		}

	}

	public bool IsOnBeat(){

		if (!musicLevel) {
			return true;
		} else {

			float tempo = metronome.tempo * 1f;
			float time = audioSource.time;
			float threshold = metronome.errorThreshold;

			float beatDuration = 60f / tempo;

			print (time % beatDuration);


			if ((time % beatDuration <= threshold) || (time % beatDuration) >= (beatDuration - threshold)) {
				print ("On beat!");
				return true;
			} else {
				print ("Not on beat.");
				return false;
			}

		}
	}
}
