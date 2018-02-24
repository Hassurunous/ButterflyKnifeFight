using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeCollision : MonoBehaviour {

	void Start() {
		gameObject.SetActive(true);

	}

	void OnCollisionEnter (Collision target) {
		if (target.gameObject.tag.Equals ("Butterfly") == true) {
			target.gameObject.SetActive(false);
			GameController.instance.playerDied (target.gameObject, Time.time);
		} 
	}
}
