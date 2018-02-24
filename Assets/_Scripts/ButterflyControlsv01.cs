using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyControlsv01 : MonoBehaviour {

	// Wing vectors. Currently, both are the same for MVP testing. 
	// In future iteration, each wing will have separate X axial rotation added to their thrust vector. 
	float wingThrust = 500.0f;
//	float leftWingTorque = -100.0f;
//	float rightWingTorque = 100.0f;
	float currentTorque;
	public Transform from;
	public Transform to;
	public float speed = 0.1F;

	// Use this for initialization
	void Start () {
//		print ("ButterflyControls.cs loaded on " + gameObject.name);
	}

	// Update is called once per frame
	void Update () {

		Rigidbody rb = GetComponent<Rigidbody> ();
//		customPID pid = new customPID (50.0f, 50.0f, 50.0f);
//		from = transform;
//		to = transform;
//		float rollAmount = 0.0f;
//		float rollSpeed = 500.0f;


		//		float dot = Vector3.Dot(transform.right, Vector3.down);
		//		rb.AddRelativeTorque(Vector3.forward*dot*100*Time.deltaTime);
		//

		//Glider Transcription
		//https://www.youtube.com/watch?v=_UvQGfddNFY

		// Flight Control variables
		float tilt = Input.GetAxis("Vertical") * 4;
		float yaw = Input.GetAxis ("Yaw");
		float roll = Input.GetAxis ("Horizontal");

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
		if (yaw != 0) {
			transform.Rotate (Vector3.up, yaw * Time.deltaTime * 10, Space.World);
		}

		//End Glider Transcription

		// Camera should remain relatively stable when following.
		// Using moveCameTo and bias, we move the camera smoothly to track butterfly in flight


		// Control Mapping
		if (Input.GetKeyDown ("o") && Input.GetKeyDown ("p")) {
			rb.AddForce (transform.forward * wingThrust / 2 + transform.up * wingThrust);
		} else if (Input.GetKeyDown ("o")) {
			//			print ("Fire1 pressed.");
			rb.AddForce (transform.forward * wingThrust / 10 + transform.up * wingThrust / 10 + transform.right * wingThrust / 5);
//			rollAmount -= rollSpeed;
//			rb.AddRelativeTorque (new Vector3 (0, 0, leftWingTorque));
//			currentTorque += leftWingTorque;
//			transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, Time.time * speed);
//			transform.Rotate(0, 0, rollAmount*Time.deltaTime);
		} else if (Input.GetKeyDown ("p")) {
			rb.AddForce (transform.forward * wingThrust / 10 + transform.up * wingThrust / 10 + -transform.right * wingThrust / 5);
//			rollAmount += rollSpeed;
//			rb.AddRelativeTorque (new Vector3 (0, 0, rightWingTorque));
//			currentTorque += rightWingTorque;
//			transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, Time.time * -speed);
//			transform.Rotate(0, 0, rollAmount*Time.deltaTime);
		} else {
		}

		if (Input.GetKeyDown ("u")) {
			transform.up = Vector3.up;
		}
			
//		float pidTorqueUpdate = pid.Update (0.0f, currentTorque, 0.1f);
//		Vector3 pidUpdate = new Vector3 (0, 0, pidTorqueUpdate);
//		rb.AddRelativeTorque (pidUpdate);
//		print ("currentTorque = " + currentTorque);
//		print ("pidTorqueUpdate = " + pidTorqueUpdate);


	}

	void FixedUpdate() {
//
//		Rigidbody rb = GetComponent<Rigidbody> ();
//
//
//		// Check the object's angular momentum and adjust it to level out.
////		print("rb.angularVelocity: " + rb.angularVelocity);
//
//		customPID pid = new customPID (50.0f, 50.0f, 50.0f);
//		//		print("PID Update:" + pid.Update (0.0f, rb.angularVelocity.x, 5));
//		float pidXUpdate = pid.Update(0.0f, rb.angularVelocity.x, 0.1f);
//		float pidYUpdate = pid.Update(0.0f, rb.angularVelocity.y, 0.1f);
//		float pidZUpdate = pid.Update(0.0f, rb.angularVelocity.z, 0.1f);
//
		//PID the X rotation if the butterfly is not on the ground
//		float terrainHeightPlaneLocale = Terrain.activeTerrain.Sample;

	}
}