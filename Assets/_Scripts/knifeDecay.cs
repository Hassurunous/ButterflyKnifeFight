using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knifeDecay : MonoBehaviour {
	private float lifeSpan;
	private float elapsedTime = 0.0f;
	public GameObject lastOwner;
	public bool held = false;

	// Use this for initialization
	void Start () {
		if (transform.parent != null) {
			gameObject.GetComponent<knifeDecay> ().enabled = false;
		}
		lifeSpan = Random.Range (25, 30);
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		if (elapsedTime >= lifeSpan) {
			Destroy (gameObject);
		} else if (elapsedTime >= 10.0f) {
			gameObject.GetComponent<knifeDecay> ().lastOwner = null;
		}
	}
}
