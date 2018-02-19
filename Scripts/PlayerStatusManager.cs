using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum MovementState{
	Grounded,
	OnWallLeft,
	OnWallRight,
	Jumping,
	Paused
}

public class PlayerStatusManager : MonoBehaviour {

	public Vector3 startLocation;
	private Animator animator;

	private PowerupManager powerupManager;
	private MetronomeManager metronomeManager;
	private TrailManager trailManager;
	private CameraController cameraController;
	private JumpTypeManager jumpTypeManager;
	private PlayerAppearanceManager appearanceManager;
	private ProgressManager progManager;
	private PlayerCounter counter;

	private bool alive;
	private bool inWater = false;
	private bool slippery;
	private int jumps;

	private int tripletCounter;
	private bool tripletJumpUsed;

	private MovementState currentState;

	public MovementState CurrentState{
		get 
		{
			return currentState;
		}
		set 
		{
			currentState = value;
			appearanceManager.UpdateAppearanceFromMovementState (value);
		}
	}


	// Use this for initialization
	void Start ()
	{
		startLocation = transform.position;
		animator = GetComponent<Animator> ();
		alive = false;
		slippery = false;
		jumps = 0;

		jumpTypeManager = FindObjectOfType<JumpTypeManager> ();
		if (!jumpTypeManager) {
			Debug.Log ("Unable to find JumpTypeManager!");
		}

		powerupManager = FindObjectOfType<PowerupManager> ();
		if (!powerupManager) {
			Debug.Log ("Unable to find powerup manager!");		
		}

		metronomeManager = FindObjectOfType<MetronomeManager> ();
		if (!metronomeManager) {
			Debug.Log ("Unable to find metronome manager!");		
		}

		appearanceManager = FindObjectOfType<PlayerAppearanceManager> ();
		if (!appearanceManager) {
			Debug.LogError ("Unable to find appearance manager!");		
		}
	
		trailManager = FindObjectOfType<TrailManager> ();
		if (!trailManager) {
			Debug.Log ("Unable to find trail manager!");		
		}

		cameraController = FindObjectOfType<CameraController> ();
		if (!cameraController) {
			Debug.Log ("Unable to find camera controller!");		
		}

		progManager = FindObjectOfType<ProgressManager> ();
		if (!progManager) {
			Debug.Log ("Unable to find ProgressManager.");
		}

		counter = FindObjectOfType<PlayerCounter> ();
		if (!counter) {
			Debug.Log ("Unable to find PlayerCounter.");
		}
	}
	
	public void Kill ()
	{
		//Debug.Log("told to kill");
		if (alive) {
			Debug.Log("Killed!");
			alive = false;
			Invoke("Respawn", 1f);
			animator.SetTrigger("death");
			if (metronomeManager.musicLevel) {
				//metronomeManager.Reset ();
				trailManager.StopTrail ();
				trailManager.SpawnTrail ();
			}
			cameraController.SetSizeToDefault ();
		}

	}

	public bool IsAlive ()
	{
		return alive;
	}

	private void Respawn ()
	{
		if (metronomeManager.musicLevel) {
			powerupManager.LoadState ();
		}

		transform.position = startLocation;
		animator.SetTrigger ("respawn");
		StartLevel ();

	}

	public void StartLevel ()
	{
		alive = true;
	}

	public void ToggleActive(){
		alive = !alive;
	}
		

	private int GetMaxJumps(){
		JumpType j = jumpTypeManager.getJumpType ();
		int refreshJumps = 0;

		switch (j) {
		case JumpType.Eighth:
			refreshJumps = 7;
			break;
		case JumpType.Quarter:
			refreshJumps = 3;
			break;
		case JumpType.Half:
			refreshJumps = 1;
			break;
		case JumpType.Whole:
			refreshJumps = 0;
			break;
		default:
			//print ("No jump types available.");
			break;
		}
		return refreshJumps;
	}

	public void StartTouching (string name)
	{
		switch (name) {
		case "Feet":
			CurrentState = MovementState.Grounded;
			jumps = 0;
			tripletJumpUsed = false;
			break;
		case "Left":
			if (CurrentState != MovementState.Grounded && !slippery) {
				CurrentState = MovementState.OnWallLeft;
				jumps = 0;
				tripletJumpUsed = false;

			}
			break;
		case "Right":
			if (CurrentState != MovementState.Grounded && !slippery) {
				CurrentState = MovementState.OnWallRight;
				jumps = 0;
				tripletJumpUsed = false;

			}
			break;
		default:
			break;
		}
	}

	public void EndTouching (string name)
	{
		switch (name) {
		case "Feet":
			if (CurrentState != MovementState.OnWallLeft && CurrentState != MovementState.OnWallRight) {
				CurrentState = MovementState.Jumping;
			}
			break;
		case "Left":
			if (CurrentState != MovementState.Grounded) {
				CurrentState = MovementState.Jumping;
			}
			break;
		case "Right":
			if (CurrentState != MovementState.Grounded) {
				CurrentState = MovementState.Jumping;
			}
			break;
		default:
			break;
		}

	}

	public void ChangeSlipStatus(bool inSlipRegion){
		slippery = inSlipRegion;
	}

	public void ResetTriplets(){
		tripletCounter = 0;
	}

	public bool CanJumpAgain(){
		if (currentState == MovementState.Jumping) {
			appearanceManager.DoubleJump ();
		}

		return (jumps <= GetMaxJumps ());
	}

	public bool UseJumpOnBeat(){
		jumps++;
		if (counter.IsOnBeat ()) {
			counter.Hit ();
		} else {
			counter.Miss ();
		}


		return (counter.IsOnBeat ());
	}

	public bool TripletJump(){
		tripletCounter++;
		print (tripletCounter);
		if (tripletCounter == 2 && !tripletJumpUsed && counter.ActiveBeatHitBefore ()) {
			tripletCounter = 0;
			appearanceManager.TripletJump ();
			return true;
		}
		return false;
	}
}


