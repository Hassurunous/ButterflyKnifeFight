using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Button> ().onClick.AddListener(() => GlobalController.startGame());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
