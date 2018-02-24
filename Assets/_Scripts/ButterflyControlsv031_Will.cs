using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyControlsv031_Will: MonoBehaviour {

	private enum ControlsState {LoadingControls, Ready};
	private ControlsState currentState;

	float wingThrust = 5.0f;
	private Rigidbody rb;
	private float timeFire1Pressed;
	private float timeFire2Pressed;
	Vector3 movementSpeed = new Vector3(0.0f, 1.0f, 0.0f);
	public float movementXSpeed = 0.0f;
	public float movementYSpeed;
	public float movementZSpeed;
	float gravity = 40.0f;
	//	bool addForce = false;
	private bool L_isAxisInUse = false;
	private bool R_isAxisInUse = false;
	private bool both_flap_active = false;
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
	public int killCount = 0;
	private float lift = 0.0f;
	public GameObject knife;
	Animator flapWingsAnim;
	int flapBothHash = Animator.StringToHash ("BothFlap");
	int flapLeftHash = Animator.StringToHash ("LeftFlap");
	int flapRightHash = Animator.StringToHash ("RightFlap");

	public string horizontalAxis;
	public string verticalAxis;
	public string flapLAxis;
	public string flapRAxis;
	public string bombsAwayAxis;

	// Use this for initialization
	void Awake () {	
		//		print ("ButterflyControls.cs loaded on " + gameObject.name);
		horizontalAxis = "";
		verticalAxis = "";
		flapLAxis = "";
		flapRAxis = "";
		bombsAwayAxis = "";
		currentState = ControlsState.LoadingControls;
		foreach (Transform child in transform) 
			if (child.CompareTag ("Butterfly")) {
				flapWingsAnim = child.GetComponent<Animator> ();
			}
	}

	// Update is called once per frame
	void Update () {
		switch (currentState) {
		case ControlsState.LoadingControls:
			if (horizontalAxis != "" && verticalAxis != "" && flapLAxis != "" && flapRAxis != "" && bombsAwayAxis != "") {
				currentState = ControlsState.Ready;
			}
			break;
		case ControlsState.Ready:
			rb = GetComponent<Rigidbody> ();

			//		movementXSpeed = movementSpeed.x;
			movementYSpeed = movementSpeed.y;
			movementZSpeed = movementSpeed.z;

			if (movementSpeed.z > 250.0f) {
				movementSpeed.z = 250.0f;
			}
			if (movementSpeed.y > 50.0f) {
				movementSpeed.y = 50.0f;
			}

			//		// Camera should remain relatively stable when following.
			//		// Using moveCameTo and bias, we move the camera smoothly to track butterfly in flight
			//		Vector3 moveCamTo = transform.position + Vector3.up * 10.0f + -transform.forward * 20.0f;
			//		float bias = 0.98f;
			//		Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
			//		Camera.main.transform.LookAt( transform.position + transform.forward * 3.0f );

			// Control Mapping
			// Which wing(s) to flap
			if( (Input.GetAxis(flapLAxis) > 0 || Input.GetButtonDown(flapLAxis))) {
				if(L_isAxisInUse == false)
				{
					timeFire1Pressed = Time.time;
					flapWings ();
					//				L_isAxisInUse = true;
				}
			}
			if( Input.GetAxisRaw(flapLAxis) <= 0)
			{
				//			print ("FlapL == " + Input.GetAxisRaw("FlapL"));
				L_isAxisInUse = false;
				both_flap_active = false;
			}    
			if (Input.GetAxis(flapRAxis) > 0 || Input.GetButtonDown(flapRAxis)) {
				//	print ("FlapR == " + Input.GetAxisRaw("FlapR"));
				if(R_isAxisInUse == false)
				{
					timeFire2Pressed = Time.time;
					flapWings ();
					//				R_isAxisInUse = true;
				}
			}
			if( Input.GetAxisRaw(flapRAxis) <= 0)
			{
				//			print ("FlapR == " + Input.GetAxisRaw("FlapR"));
				R_isAxisInUse = false;
				both_flap_active = false;
			} 

			if (Input.GetButtonDown(bombsAwayAxis)) {
//				print("Bomb's away!");
				//			gameObject.transform.GetChild(2).gameObject.SetActive(false);
				foreach (Transform child in transform) 
					if (child.CompareTag ("Knife")) {
						if (child.gameObject.activeSelf == true) {
							GameObject knifeClone = Instantiate(knife, transform.position - transform.up * 5.0f, transform.rotation * Quaternion.Euler(0, 90, 0));
							knifeClone.GetComponent<Rigidbody> ().velocity = transform.forward * movementZSpeed * 3f + transform.forward * 100.0f;
							child.gameObject.SetActive (false);
						}
					}
			}

			//Glide
			Gliding();
			//		if ((Input.GetButton ("FlapL") && Input.GetButton ("FlapR")) || (Input.GetAxis("FlapL") > 0 && Input.GetAxis("FlapR") > 0)) {
			//			Gliding ();
			//		} else {
			//			Gliding ();
			//		}


			break;
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
		//		movementXSpeed += wingThrust / 10;
		//		transform.position += transform.right * Time.deltaTime * movementXSpeed;
		iTween.RotateBy (gameObject, iTween.Hash (
			"z", -0.01,
			"time", 0.01f,
			"easeType", "easeOutQuad"));
		flapWingsAnim.SetTrigger(flapLeftHash);
	}

	public void RightFlap() {
		rb = GetComponent<Rigidbody> ();
		//		rb.AddForce (transform.forward * wingThrust / 6 + transform.up * wingThrust / 6 + -transform.right * wingThrust);
		//		rb.AddForce(-transform.right * wingThrust / 2 );
		//		movementXSpeed -= wingThrust / 10;
		//		transform.position += transform.right * Time.deltaTime * movementXSpeed;
		iTween.RotateBy (gameObject, iTween.Hash (
			"z", 0.01,
			"time", 0.01f,
			"easeType", "easeOutQuad"));
		flapWingsAnim.SetTrigger(flapRightHash);
	}

	public void BothFlap() {
		rb = GetComponent<Rigidbody> ();
		//		print (both_flap_active);
		//		rb.AddForce (transform.forward * wingThrust / 2 + transform.up * wingThrust);
		if (both_flap_active == false) {
			movementSpeed.z += wingThrust / 4;
			movementSpeed.y += wingThrust;
			flapWingsAnim.SetTrigger(flapBothHash);
		}

		both_flap_active = true;
		//		transform.position += transform.forward * Time.deltaTime * movementZSpeed + transform.up * Time.deltaTime * wingThrust * 10.0f;
	}

	void flapWings() {
		if (Mathf.Abs (timeFire2Pressed - timeFire1Pressed) < 0.3f) {
			BothFlap ();
		} else {
			if (timeFire1Pressed > timeFire2Pressed) {
				RightFlap ();
			} else if (timeFire2Pressed > timeFire1Pressed) {
				LeftFlap ();
			}
		}
	}

	void Gliding() {

		//		print ("Gliding...");
		rb = GetComponent<Rigidbody> ();
		rb.angularVelocity = Vector3.zero;


		//Glider Transcription
		//https://www.youtube.com/watch?v=_UvQGfddNFY

		// Flight Control variables
		float tilt = Input.GetAxis(verticalAxis) * 12;
		float yaw = Input.GetAxis (horizontalAxis) * 8;


		if ((Input.GetButton (flapLAxis) && Input.GetButton (flapRAxis)) || (Input.GetAxis(flapLAxis) > 0 && Input.GetAxis(flapRAxis) > 0)) {
			movementZSpeed -= 10.0f * Time.deltaTime;
		} 


		if (tilt != 0) {
			transform.Rotate (transform.right, tilt * Time.deltaTime * 10, Space.World);
		}
		if (yaw != 0) {
			transform.Rotate (transform.up, yaw * Time.deltaTime * 10, Space.World);
		}

		//End Glider Transcription

		// Stop butterfly movement if it lands on the ground.
		float terrainHeightPlaneLocale = Terrain.activeTerrain.SampleHeight (transform.position);
		// print(terrainHeightPlaneLocale + " + " + transform.position.y);

		if (terrainHeightPlaneLocale + 3 >= transform.position.y && !is_Landed) {
			movementSpeed.x = 0.0f;
			movementSpeed.z = 0.0f;
			movementSpeed.y = 0.0f;
			is_Landed = true;
			//			print ("is_Landed = " + is_Landed);
		} else {
			if (is_Landed == true && terrainHeightPlaneLocale + 5 <= transform.position.y) {
				is_Landed = false;
				//				print ("is_Landed = " + is_Landed);
			}
			if (terrainHeightPlaneLocale + 3 < transform.position.y) {
				//				transform.position -= Vector3.up * gravity * Time.deltaTime;
				movementSpeed -= transform.InverseTransformDirection(Vector3.up) * gravity * Time.deltaTime;
				lift = transform.TransformDirection (transform.up).y * (movementZSpeed / 150);
				//				print ("lift = " + lift);
				movementSpeed.y += lift * gravity * Time.deltaTime;
				//				movementSpeed -= transform.InverseTransformDirection(Vector3.up) * gravity * Time.deltaTime - (transform.InverseTransformDirection(Vector3.up) * gravity * Time.deltaTime) * (movementZSpeed / 100);
			}
			transform.position += transform.up * movementSpeed.y * Time.deltaTime;
			transform.position += transform.forward * Time.deltaTime * movementSpeed.z;
			transform.position += transform.right * Time.deltaTime * movementXSpeed;

			// Yaw based on how we're rolled.
			float tip = (transform.right + Vector3.up).magnitude - 1.414214f;
			yaw -= tip;
		}

		//		transform.Rotate (roll / 3, -yaw, -tilt / 3);
	}
}
