using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleZoneTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider target) {
		if (target.gameObject.tag.Equals ("Butterfly") == true) {
			print ("entered");
			target.transform.Rotate(180,180,180);
		}
	}
}
