using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour {

	public Text countDownText;
	public GameObject player;

	private MetronomeManager metronome;
	private AudioSource audioSource;
	private int timeToStart;

	void Start ()
	{
		metronome = FindObjectOfType<MetronomeManager> ();
		if (!metronome) {
			Debug.Log ("MusicManager can't find MetronomeManager.");
		}

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
			audioSource.Play();
			metronome.Downbeat ();
			player.GetComponent<PlayerStatusManager>().StartLevel();
			Invoke ("CountDown", 1f);

		} else if (timeToStart < 0) {
			countDownText.text = "";
			timeToStart = 3;
		}

	}

	public bool IsOnBeat(){
		float tempo = metronome.tempo * 1f;
		float time = audioSource.time;
		float threshold = metronome.errorThreshold;

		float beatDuration = 60f / tempo;

		print (time % beatDuration);


		if ((time % beatDuration <= threshold) || (time % beatDuration) >= (beatDuration - threshold)){
			print ("On beat!");
			return true;
		} else {
			print ("Not on beat.");
			return false;
		}


	}
}
