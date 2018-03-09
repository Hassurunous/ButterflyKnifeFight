using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		print ("Please quit?");
		gameObject.GetComponent<Button> ().onClick.AddListener(() => GlobalController.QuitGame());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
