using UnityEngine;
using System.Collections;

public class PlayerStatusManager : MonoBehaviour {


	public Vector3 startLocation;
	public MusicManager musicManager;

	private Animator animator;
	private bool alive;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	public void Kill ()
	{
		Debug.Log("told to kill");
		if (alive) {
			Debug.Log("killed!");
			alive = false;
			Invoke("Respawn", 2f);
			animator.SetTrigger("death");
			musicManager.GetComponent<AudioSource>().Stop();
		}

	}

	public bool IsAlive ()
	{
		return alive;
	}

	private void Respawn ()
	{
		this.transform.position = startLocation;
		animator.SetTrigger("respawn");
		musicManager.StartMusic();

	}

	public void StartLevel ()
	{
		alive = true;
	}
}
