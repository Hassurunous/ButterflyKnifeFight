using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyPhysics : MonoBehaviour {

	// Wing vectors. Currently, both are the same for MVP testing. 
	// In future iteration, each wing will have separate X axial rotation added to their thrust vector. 
	float wingThrust = 100.0f;
//	float leftWingTorque = -100.0f;
//	float rightWingTorque = 100.0f;
	float currentTorque;
//	int timer = 0;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		print ("ButterflyControls.cs loaded on " + gameObject.name);
	}

	// Update is called once per frame
	void Update () {

		rb = GetComponent<Rigidbody> ();

		//Glider Transcription
		//https://www.youtube.com/watch?v=_UvQGfddNFY

		// Flight Control variables
		float tilt = Input.GetAxis("Vertical") * 4;
//		float yaw = Input.GetAxis ("Yaw");
		float roll = Input.GetAxis ("Horizontal") * 4;


		// Yaw based on how we're rolled.
		//		float tip = (transform.right + Vector3.up).magnitude - 1.414214f;
		//		yaw -= tip;

		// Tip based on if we are going backwards.
		if ((transform.forward + rb.velocity).magnitude < 1.4) {
			tilt += 0.3f;
		}

		if (tilt != 0) {
			transform.Rotate (transform.right, tilt * Time.deltaTime * 10, Space.World);
		}
		if (roll != 0) {
			transform.Rotate (transform.forward, roll * Time.deltaTime * -10, Space.World);
		}
//		if (yaw != 0) {
//			transform.Rotate (Vector3.up, yaw * Time.deltaTime * 10, Space.World);
//		}

		//End Glider Transcription

		// Camera should remain relatively stable when following.
		// Using moveCameTo and bias, we move the camera smoothly to track butterfly in flight
			

	}

	void FixedUpdate() {
		
		rb = GetComponent<Rigidbody> ();

		// Stop butterfly movement if it lands on the ground.
		float terrainHeightPlaneLocale = Terrain.activeTerrain.SampleHeight (transform.position);
		print(terrainHeightPlaneLocale + " + " + transform.position.y);

		if (terrainHeightPlaneLocale + 2 >= transform.position.y) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}

	}

	public void LeftFlap() {
		rb = GetComponent<Rigidbody> ();
		rb.AddForce (transform.forward * wingThrust / 6 + transform.up * wingThrust / 6 + transform.right * wingThrust);
		iTween.RotateBy (gameObject, iTween.Hash (
			"z", -0.02,
			"time", 0.3f,
			"easeType", "easeOutQuad"));
	}

	public void RightFlap() {
		rb = GetComponent<Rigidbody> ();
		rb.AddForce (transform.forward * wingThrust / 6 + transform.up * wingThrust / 6 + -transform.right * wingThrust);
		iTween.RotateBy (gameObject, iTween.Hash (
			"z", 0.02,
			"time", 0.3f,
			"easeType", "easeOutQuad"));
	}

	public void BothFlap() {
		rb = GetComponent<Rigidbody> ();
		rb.AddForce (transform.forward * wingThrust / 2 + transform.up * wingThrust);
	}
}
