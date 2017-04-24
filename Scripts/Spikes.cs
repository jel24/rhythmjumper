using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {


	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			coll.GetComponentInParent<PlayerStatusManager>().Kill();
		}
	}
}
