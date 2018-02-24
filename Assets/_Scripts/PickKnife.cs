using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickKnife : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision target) {
		if (target.gameObject.tag.Equals ("Butterfly") == true) {
			Destroy (gameObject);
			target.gameObject.transform.GetChild(0).gameObject.SetActive(true);
		} 
	}
}
