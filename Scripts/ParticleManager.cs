using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleType{
	Phoenix,
	JumpSuccess,
	JumpFailure,
	Triplet,
	Bubbles
}

public class ParticleManager : MonoBehaviour {


	public Object PhoenixParticle;
	public Object JumpSuccessParticle;
	public Object JumpFailureParticle;
	public Object TripletParticle;
	public Object BubbleParticle;

	private Dictionary<ParticleType, Object> particles;


	// Use this for initialization
	void Start () {
		particles = new Dictionary<ParticleType, Object> ();
		particles.Add (ParticleType.Phoenix, PhoenixParticle);
		particles.Add (ParticleType.JumpSuccess, JumpSuccessParticle);
		particles.Add (ParticleType.JumpFailure, JumpFailureParticle);
		particles.Add (ParticleType.Triplet, TripletParticle);
		particles.Add (ParticleType.Bubbles, BubbleParticle);



	}

	public Object GetParticle(ParticleType p){
		return particles[p];
	}

}
