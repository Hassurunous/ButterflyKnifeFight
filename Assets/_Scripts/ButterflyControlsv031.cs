using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButterflyControlsv031 : MonoBehaviour {

	private enum ControlsState {LoadingControls, Ready};
	private ControlsState currentState;
	private Rigidbody rb;
	private float timeFire1Pressed;
	private float timeFire2Pressed;
	private float wingThrust = 10.0f;
	private float gravity = 40.0f;
	private bool L_isAxisInUse = false;
	private bool R_isAxisInUse = false;
	private bool both_flap_active = false;
	private bool is_Landed = false;
	private Vector3 rb_velocity;
	private Vector3 offset;
	private float lift = 0.0f;
	private GameController gameController;
	Animator flapWingsAnim;
	int flapBothHash = Animator.StringToHash ("BothFlap");
	int flapLeftHash = Animator.StringToHash ("LeftFlap");
	int flapRightHash = Animator.StringToHash ("RightFlap");
	private bool flapWingsBool = false;

	public Vector3 movementSpeed = new Vector3(0.0f, 1.0f, 0.0f);
	public float movementXSpeed = 0.0f;
	public float movementYSpeed;
	public float movementZSpeed;
	public float bias = 0.85f;
	public float followDistance = 10.0f;
	public float followHeight = 2.0f;
	public float cameraVertOffsetMax = 10.0f;
	public float cameraVertOffsetMin = 5.0f;
	public float cameraFollowOffsetMax = 40.0f;
	public float cameraFollowOffsetMin = 20.0f;
	public int killCount = 0;
	public bool killMsgActive = false;
	public float killTime = 0.0f;
	public GameObject knife;
	public Text warningMsg;


	public string horizontalAxis;
	public string verticalAxis;
	public string flapLAxis;
	public string flapRAxis;
	public string bombsAwayAxis;
	public string pauseButton;

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
		gameController = GameObject.Find ("Game Controller").gameObject.GetComponent<GameController> ();
		switch (currentState) {
		case ControlsState.LoadingControls:
			if (horizontalAxis != "" && verticalAxis != "" && flapLAxis != "" && flapRAxis != "" && bombsAwayAxis != "") {
				currentState = ControlsState.Ready;
			}
			break;
		case ControlsState.Ready:
			rb = GetComponent<Rigidbody> ();

			movementYSpeed = movementSpeed.y;
			movementZSpeed = movementSpeed.z;

			// Control Mapping
			// Which wing(s) to flap
			if ((Input.GetAxis (flapLAxis) > 0 || Input.GetButtonDown (flapLAxis))) {
				if (L_isAxisInUse == false) {
					timeFire1Pressed = Time.time;
//					flapWings ();
					flapWingsBool = true;
				}
			}
			if (Input.GetAxisRaw (flapLAxis) <= 0) {
				L_isAxisInUse = false;
				both_flap_active = false;
			}    
			if (Input.GetAxis (flapRAxis) > 0 || Input.GetButtonDown (flapRAxis)) {
				if (R_isAxisInUse == false) {
					timeFire2Pressed = Time.time;
//					flapWings ();
					flapWingsBool = true;
				}
			}
			if (Input.GetAxisRaw (flapRAxis) <= 0) {
				R_isAxisInUse = false;
				both_flap_active = false;
			} 

			if (Input.GetButtonDown (bombsAwayAxis)) {
				BombsAway ();

			}


			if (Input.GetButtonDown (pauseButton)) {
				print ("Pause pressed");
				gameController.gamePaused = !gameController.gamePaused;
				print (gameController.gamePaused);
			}
				

			if (killMsgActive) {
				if (Time.time > killTime + 2.0f) {
					warningMsg.text = "";
				}
			}
				
			break;
		}
	}

	void FixedUpdate() {
		Gliding ();

	}

	public void LeftFlap() {
		//rb = GetComponent<Rigidbody> ();
		//		rb.AddForce (transform.forward * wingThrust / 6 + transform.up * wingThrust / 6 + transform.right * wingThrust);
		//		rb.AddForce(transform.right * wingThrust / 2);
		//		movementXSpeed += wingThrust / 10;
		//		transform.position += transform.right * Time.deltaTime * movementXSpeed;
//		iTween.RotateBy (gameObject, iTween.Hash (
//			"z", -0.01,
//			"time", 0.01f,
//			"easeType", "easeOutQuad"));
		transform.Rotate (transform.forward, -Time.fixedDeltaTime * 100, Space.World);
		flapWingsAnim.SetTrigger(flapLeftHash);
	}

	public void RightFlap() {
		//rb = GetComponent<Rigidbody> ();
		//		rb.AddForce (transform.forward * wingThrust / 6 + transform.up * wingThrust / 6 + -transform.right * wingThrust);
		//		rb.AddForce(-transform.right * wingThrust / 2 );
		//		movementXSpeed -= wingThrust / 10;
		//		transform.position += transform.right * Time.deltaTime * movementXSpeed;
//		iTween.RotateBy (gameObject, iTween.Hash (
//			"z", 0.01,
//			"time", 0.01f,
//			"easeType", "easeOutQuad"));
		transform.Rotate (transform.forward, Time.fixedDeltaTime * 100, Space.World);
		flapWingsAnim.SetTrigger(flapRightHash);
	}

	public void BothFlap() {
		//rb = GetComponent<Rigidbody> ();
		//		print (both_flap_active);
		//		rb.AddForce (transform.forward * wingThrust / 2 + transform.up * wingThrust);
		if (both_flap_active == false) {

			if (movementSpeed.z < 400.0f) {
				movementSpeed.z += wingThrust / 4;
			}
			if (movementSpeed.y < 50.0f) {
				movementSpeed.y += wingThrust;
			}

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

		// Flap wings
		if (flapWingsBool == true) {
			flapWings ();
			flapWingsBool = false;
		}

		//Glider Transcription
		//https://www.youtube.com/watch?v=_UvQGfddNFY

		// Flight Control variables
		float tilt = Input.GetAxis(verticalAxis) * 12;
		float yaw = Input.GetAxis (horizontalAxis) * 8;

		//What is the purpose of these lines?
//		if ((Input.GetButton (flapLAxis) && Input.GetButton (flapRAxis)) || (Input.GetAxis(flapLAxis) > 0 && Input.GetAxis(flapRAxis) > 0)) {
//			movementZSpeed -= 10.0f * Time.deltaTime;
//		} 


		if (tilt != 0) {
			transform.Rotate (transform.right, tilt * Time.deltaTime * 10, Space.World);
		}
		if (yaw != 0) {
			transform.Rotate (transform.up, yaw * Time.deltaTime * 10, Space.World);
		}

		//End Glider Transcription

		// Stop butterfly movement if it lands on the ground.
		float terrainHeightPlaneLocale = Terrain.activeTerrain.SampleHeight (transform.position);

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
				movementSpeed -= transform.InverseTransformDirection(Vector3.up) * gravity * Time.deltaTime;
				lift = transform.TransformDirection (transform.up).y * (movementZSpeed / 150);
				if (movementSpeed.y < 50.0f) {
					movementSpeed.y += lift * gravity * Time.deltaTime;
				}
			}
			rb.MovePosition(transform.position + (transform.up * movementSpeed.y * Time.fixedDeltaTime) + (transform.forward * movementSpeed.z * Time.fixedDeltaTime) + (transform.right * movementXSpeed * Time.fixedDeltaTime));

//			transform.position += transform.up * movementSpeed.y * Time.deltaTime;
//			transform.position += transform.forward * Time.deltaTime * movementSpeed.z;
//			transform.position += transform.right * Time.deltaTime * movementXSpeed;

			// Yaw based on how we're rolled.
			float tip = (transform.right + Vector3.up).magnitude - 1.414214f;
			yaw -= tip;
		}

		//		transform.Rotate (roll / 3, -yaw, -tilt / 3);
	}

	void BombsAway() {
		foreach (Transform child in transform)
			if (child.CompareTag ("Knife")) {
				if (child.gameObject.activeSelf == true) {
					GameObject knifeClone = Instantiate (knife, transform.position - transform.up * 5.0f, transform.rotation * Quaternion.Euler (0, 90, 0));
					knifeClone.GetComponent<Rigidbody> ().velocity = transform.forward * movementZSpeed * 3f + transform.forward * 100.0f;
					knifeClone.GetComponent<knifeDecay> ().lastOwner = gameObject;
					child.gameObject.SetActive (false);
				}
			}
	}
}
