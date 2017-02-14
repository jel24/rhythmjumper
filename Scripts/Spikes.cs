using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

	void Start(){			
		if (gameObject.name.Contains("Cloud")) {
			this.transform.Rotate(new Vector3(0f, 0f, (Random.Range (0f, 360f))));
		}
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			coll.GetComponentInParent<PlayerStatusManager>().Kill();
		}
	}
}
