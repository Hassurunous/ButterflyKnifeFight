using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyControlsv02 : MonoBehaviour {

	float wingThrust = 1.0f;
	private Rigidbody rb;
	private float timeFire1Pressed;
	private float timeFire2Pressed;
	public float movementXSpeed = 1.0f;
	public float movementYSpeed = 1.0f;
	public float movementZSpeed = 0.0f;
	float gravity = 10.0f;
	//	bool addForce = false;
	private bool L_isAxisInUse = false;
	private bool R_isAxisInUse = false;
	private bool is_Landed = false;
	private Vector3 rb_velocity;
	private Vector3 offset;
	public float bias = 0.85f;
	public float followDistance = 10.0f;
	public float followHeight = 2.0f;
	public float cameraVertOffsetMax = 10.0f;
	public float cameraVertOffsetMin = 5.0f;
	public float cameraFollowOffsetMax = 40.0f;
	public float cameraFollowOffsetMin = 20.0f;

	// Use this for initialization
	void Start () {	
		print ("ButterflyControls.cs loaded on " + gameObject.name);
	}

	// Update is called once per frame
	void Update () {

		rb = GetComponent<Rigidbody> ();

//		// Camera should remain relatively stable when following.
//		// Using moveCameTo and bias, we move the camera smoothly to track butterfly in flight
//		Vector3 moveCamTo = transform.position + Vector3.up * 10.0f + -transform.forward * 20.0f;
//		float bias = 0.98f;
//		Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
//		Camera.main.transform.LookAt( transform.position + transform.forward * 3.0f );

		// Control Mapping
		// Which wing(s) to flap
		if( Input.GetAxis("FlapL") > 0 || Input.GetButtonDown("FlapL")) {
			//			print ("FlapL == " + Input.GetAxisRaw("FlapL"));
			if(L_isAxisInUse == false)
			{
				timeFire1Pressed = Time.time;
				flapWings ();
				L_isAxisInUse = true;
			}
		}
		if( Input.GetAxisRaw("FlapL") <= 0)
		{
			//			print ("FlapL == " + Input.GetAxisRaw("FlapL"));
			L_isAxisInUse = false;
		}    
		if (Input.GetAxis("FlapR") > 0 || Input.GetButtonDown("FlapR")) {
			//			print ("FlapR == " + Input.GetAxisRaw("FlapR"));
			if(R_isAxisInUse == false)
			{
				timeFire2Pressed = Time.time;
				flapWings ();
				R_isAxisInUse = true;
			}
		}
		if( Input.GetAxisRaw("FlapR") <= 0)
		{
			//			print ("FlapR == " + Input.GetAxisRaw("FlapR"));
			R_isAxisInUse = false;
		} 

		//Righting the object for testing
		if (Input.GetKeyDown ("u")) {
			transform.up = Vector3.up;
		}

		//Glide
		if (Input.GetButton ("FlapL") || Input.GetButton ("FlapR") || Input.GetAxis("FlapL") > 0 || Input.GetAxis("FlapR") > 0) {
			Gliding ();
		} else {
			Gliding ();
		}


	}

	void FixedUpdate() {
		
	}

	void LateUpdate() {
		//		float desiredAngle = transform.eulerAngles.y;
		//		Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
		//		Camera.main.transform.position = transform.position - (rotation * offset);
		//		Camera.main.transform.LookAt( transform.position + transform.forward * 3.0f );
		// Camera should remain relatively stable when following.
		// Using moveCameTo and bias, we move the camera smoothly to track butterfly in flight
		//		Vector3 moveCamTo = transform.position + transform.up * 5.0f + -transform.forward * 10.0f;

	}

	public void LeftFlap() {
		rb = GetComponent<Rigidbody> ();
		//		rb.AddForce (transform.forward * wingThrust / 6 + transform.up * wingThrust / 6 + transform.right * wingThrust);
		//		rb.AddForce(transform.right * wingThrust / 2);
		movementXSpeed += wingThrust / 10;
		//		transform.position += transform.right * Time.deltaTime * movementXSpeed;
		iTween.RotateBy (gameObject, iTween.Hash (
			"z", -0.02,
			"time", 0.1f,
			"easeType", "easeOutQuad"));
	}

	public void RightFlap() {
		rb = GetComponent<Rigidbody> ();
		//		rb.AddForce (transform.forward * wingThrust / 6 + transform.up * wingThrust / 6 + -transform.right * wingThrust);
		//		rb.AddForce(-transform.right * wingThrust / 2 );
		movementXSpeed -= wingThrust / 10;
		//		transform.position += transform.right * Time.deltaTime * movementXSpeed;
		iTween.RotateBy (gameObject, iTween.Hash (
			"z", 0.02,
			"time", 0.1f,
			"easeType", "easeOutQuad"));
	}

	public void BothFlap() {
		rb = GetComponent<Rigidbody> ();
		//		rb.AddForce (transform.forward * wingThrust / 2 + transform.up * wingThrust);
		movementZSpeed += wingThrust;
		movementYSpeed += wingThrust;
		//		transform.position += transform.forward * Time.deltaTime * movementZSpeed + transform.up * Time.deltaTime * wingThrust * 10.0f;
	}

	void flapWings() {
		if (Mathf.Abs (timeFire2Pressed - timeFire1Pressed) < 0.4f) {
			BothFlap ();
		} else {
			if (timeFire1Pressed > timeFire2Pressed) {
				LeftFlap ();
			} else if (timeFire2Pressed > timeFire1Pressed) {
				RightFlap ();
			}
		}
	}

	void Gliding() {

		//		print ("Gliding...");
		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Vector3.zero;

		if (movementYSpeed < 0) {
			movementYSpeed = movementYSpeed / 2;
		}


		//Glider Transcription
		//https://www.youtube.com/watch?v=_UvQGfddNFY

		// Flight Control variables
		float tilt = Input.GetAxis("Vertical") * 4;
		float roll = Input.GetAxis ("HorizontalR");
		float yaw = Input.GetAxis ("Horizontal") * 4;

		// Yaw based on how we're rolled.
		float tip = (transform.right + Vector3.up).magnitude - 1.414214f;
		yaw -= tip;

		if (tilt != 0) {
			transform.Rotate (transform.right, tilt * Time.deltaTime * 10, Space.World);
		}
		if (roll != 0) {
			transform.Rotate (transform.forward, roll * Time.deltaTime * -10, Space.World);
		}
		if (yaw != 0) {
			transform.Rotate (transform.up, yaw * Time.deltaTime * 10, Space.World);
		}

		//End Glider Transcription

		// Stop butterfly movement if it lands on the ground.
		float terrainHeightPlaneLocale = Terrain.activeTerrain.SampleHeight (transform.position);
		// print(terrainHeightPlaneLocale + " + " + transform.position.y);

		if (terrainHeightPlaneLocale + 3 >= transform.position.y && !is_Landed) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			transform.up = Vector3.up;
			movementXSpeed = 0.0f;
			movementZSpeed = 0.0f;
			movementYSpeed = 0.0f;
			is_Landed = true;
			print ("is_Landed = " + is_Landed);
		} else {
			if (is_Landed == true && terrainHeightPlaneLocale + 5 <= transform.position.y) {
				is_Landed = false;
				print ("is_Landed = " + is_Landed);
			}
			if (terrainHeightPlaneLocale + 3 < transform.position.y) {
				transform.position -= Vector3.up * gravity * Time.deltaTime;
			}
			transform.position += transform.up * movementYSpeed * Time.deltaTime;
			transform.position += transform.forward * Time.deltaTime * movementZSpeed;
			transform.position += transform.right * Time.deltaTime * movementXSpeed;
			if (movementZSpeed >= 0) {
				movementZSpeed -= transform.forward.y * Time.deltaTime * gravity;
			}
			if (movementYSpeed >= 0) {
				movementYSpeed -= gravity * Time.deltaTime;
			}
		}
			
		//		transform.Rotate (roll / 3, -yaw, -tilt / 3);
	}

	void Falling() {

		//		print ("Falling...");
		rb = GetComponent<Rigidbody> ();
		rb_velocity = rb.velocity;

		//Glider Transcription
		//https://www.youtube.com/watch?v=_UvQGfddNFY

		// Flight Control variables
		float tilt = Input.GetAxis("Vertical") * 12;
		float roll = -Input.GetAxis ("Horizontal") * 12;
		float yaw = Input.GetAxis ("HorizontalR") * 12;

		// Yaw based on how we're rolled.
		float tip = (transform.right + Vector3.up).magnitude - 1.414214f;
		yaw -= tip;

		if (tilt != 0) {
			transform.Rotate (transform.right, tilt * Time.deltaTime * 10, Space.World);
		}
		if (roll != 0) {
			transform.Rotate (transform.forward, roll * Time.deltaTime * 10, Space.World);
		}
		if (yaw != 0) {
			transform.Rotate (transform.up, yaw * Time.deltaTime * 10);
		}

		//End Glider Transcription

		// Stop butterfly movement if it lands on the ground.
		float terrainHeightPlaneLocale = Terrain.activeTerrain.SampleHeight (transform.position);
		// print(terrainHeightPlaneLocale + " + " + transform.position.y);

		if (terrainHeightPlaneLocale + 3 >= transform.position.y && !is_Landed) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			transform.up = Vector3.up;
			movementXSpeed = 0.0f;
			movementZSpeed = 0.0f;
			movementYSpeed = 0.0f;
			is_Landed = true;
			print ("is_Landed = " + is_Landed);
		} else {
			//			if (addForce) {
			//				rb.AddForce(transform.forward * movementZSpeed + transform.right * movementXSpeed + transform.up * movementZSpeed);
			//				addForce = false;
			//			}
			if (is_Landed == true && terrainHeightPlaneLocale + 5 <= transform.position.y) {
				is_Landed = false;
				print ("is_Landed = " + is_Landed);
			}
			if (terrainHeightPlaneLocale + 3 < transform.position.y) {
				transform.position -= Vector3.up * gravity * Time.deltaTime;
			}
			transform.position += rb_velocity;
			if (movementZSpeed >= 0) {
				movementZSpeed -= transform.forward.y * Time.deltaTime * gravity * 5.0f;
			}
		}

		//		transform.Rotate (roll / 3, -yaw, -tilt / 3);
	}
}
