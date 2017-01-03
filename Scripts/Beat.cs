using UnityEngine;
using System.Collections;

public class Beat : MonoBehaviour {

	private bool initiated;

	private Vector3 vector;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (initiated) {
			this.transform.Translate(vector * Time.deltaTime);
		}

		// 	this.GetComponent<RectTransform>().anchoredPosition = spawnPoint;
	}

	public void InitiateBeat (float beatVelocity)
	{
		initiated = true;
		vector = new Vector3(-beatVelocity, 0f, 0f);
		Invoke("Kill", 400f/beatVelocity);
	}

	private void Kill ()
	{
		Destroy(this.gameObject);
	}

	//beats [3].GetComponent<RectTransform>().anchoredPosition = targetPos;

}
