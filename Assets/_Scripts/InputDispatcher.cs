using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {
	public string leftAxisName;
	public string rightAxisName;
	public string rollAxisName;
	public string yawAxisName;
	public string pitchAxisName;
	public ButterflyControlPhysics butterflyScript;

	public bool leftIsInUse = false;
	public bool rightIsInUse = false;

	public float leftFirePressedTime = 0;
	public float rightFirePressedTime = 0;

	public void processInput() {

		if( Input.GetAxis(leftAxisName) > 0 || Input.GetButtonDown(leftAxisName)) {
			//			print ("FlapL == " + Input.GetAxisRaw("FlapL"));
			if(leftIsInUse == false)
			{
				leftFirePressedTime = Time.time;
				flapWings ();
				leftIsInUse = true;
			}
		}
		if( Input.GetAxisRaw(leftAxisName) <= 0)
		{
			//			print ("FlapL == " + Input.GetAxisRaw("FlapL"));
			leftIsInUse = false;
		}    
		if (Input.GetAxis(rightAxisName) > 0 || Input.GetButtonDown(rightAxisName)) {
			//			print ("FlapR == " + Input.GetAxisRaw("FlapR"));
			if(rightIsInUse == false)
			{
				rightFirePressedTime = Time.time;
				flapWings ();
				rightIsInUse = true;
			}
		}
		if( Input.GetAxisRaw(rightAxisName) <= 0)
		{
			//			print ("FlapR == " + Input.GetAxisRaw("FlapR"));
			rightIsInUse = false;
		} 

		//Righting the object for testing
		if (Input.GetKeyDown ("u")) {
			butterflyScript.reorientateUp ();
		}

		//Glide
		/*if (Input.GetButton (leftAxisName) || Input.GetButton (rightAxisName) || Input.GetAxis(leftAxisName) > 0 || Input.GetAxis(rightAxisName) > 0) {
			Gliding ();
		} else {
			Gliding ();
		}*/

		butterflyScript.advance();
	}

	void flapWings() {
		if (Mathf.Abs (leftFirePressedTime - rightFirePressedTime) < 0.4f) {
			butterflyScript.bothFlap ();
		} else {
			if (leftFirePressedTime > rightFirePressedTime) {
				butterflyScript.leftFlap ();
			} else if (rightFirePressedTime > leftFirePressedTime) {
				butterflyScript.rightFlap ();
			}
		}
	}
}

public class InputDispatcher : MonoBehaviour {
	
	public string[] leftAxisNames = new string[0];
	public string[] rightAxisNames = new string[0];
	public GameObject[] butterflyObjects = new GameObject[0];

	private PlayerData[] players;

	public float allowedTimeBetweenButtons = 0.4f; //tweak as needed
	private float timeFire1Pressed;
	private float timeFire2Pressed;

	// Use this for initialization
	void Start () {

		if (leftAxisNames.Length != rightAxisNames.Length || leftAxisNames.Length != butterflyObjects.Length) {
			print ("lengths of arrays in control dispatcher not equal. controls will not work.");
			players = new PlayerData[0];
		}
		else {
			int length = butterflyObjects.Length;
			players = new PlayerData[length];

			for (var i = 0; i < length; i++) {
				players[i] = new PlayerData();
				players[i].leftAxisName = leftAxisNames[i];
				players[i].rightAxisName = rightAxisNames[i];
				players[i].butterflyScript = butterflyObjects[i].GetComponent<ButterflyControlPhysics>();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		for (var i = 0; i < players.Length; i++) {
			players [i].processInput();
		}
	}


}
