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
	public Vector3 maxVelocities = new Vector3(0f, 50.0f, 400.0f);
	public float movementXSpeed = 0.0f;
	public float movementYSpeed;
	public float movementZSpeed;


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

	public float flapRotationStrength = 1.5f;

	Quaternion center;

	// Clamp rotation while on the ground.
	Vector3 groundRotationClamp = new Vector3(45.0f, 0, 45.0f);

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
		gameController = GameObject.Find ("Game Controller").gameObject.GetComponent<GameController> ();
		center = transform.rotation;
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

			movementYSpeed = movementSpeed.y;
			movementZSpeed = movementSpeed.z;

			// Control Mapping
			// Which wing(s) to flap
			if ((Input.GetAxis (flapLAxis) > 0 || Input.GetButtonDown (flapLAxis))) {
				if (L_isAxisInUse == false) {
					timeFire1Pressed = Time.time;
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

	void LateUpdate() {
		// This clamps the X and Z rotation of the butterflies while they are "is_Landed"
		if (is_Landed) {
			Vector3 currentRotation = transform.localRotation.eulerAngles;
			if (!(currentRotation.x >= 315f && currentRotation.x <= 360f) && !(currentRotation.x >= 0f && currentRotation.x <= 45f )) {
				if (currentRotation.x > 180) {
					currentRotation.x = Mathf.Clamp (currentRotation.x, 315, 360);
				} else if (currentRotation.x < 180) {
					currentRotation.x = Mathf.Clamp (currentRotation.x, 0, 45);
				}
			}
			if (!(currentRotation.z >= 315f && currentRotation.z <= 360f) && !(currentRotation.z >= 0f && currentRotation.z <= 45f )) {
				if (currentRotation.z > 180) {
					currentRotation.z = Mathf.Clamp (currentRotation.z, 315, 360);
				} else if (currentRotation.x < 180) {
					currentRotation.z = Mathf.Clamp (currentRotation.z, 0, 45);
				}
			}
			transform.localRotation = Quaternion.Euler (currentRotation);
		}
	}

	public void LeftFlap() {

		transform.Rotate (transform.forward, -flapRotationStrength, Space.World);
		flapWingsAnim.SetTrigger (flapLeftHash);

//		Quaternion zQuaternion = Quaternion.AngleAxis(-flapRotationStrength, transform.forward);
//		Quaternion temp = transform.localRotation * zQuaternion;
//
//		// If the butterfly is on the ground, clamp its rotation. 
//		if (!is_Landed) {
//			transform.localRotation = temp;
//			flapWingsAnim.SetTrigger (flapLeftHash);
//		} else if (Quaternion.Angle (center, temp) <= 45.0f) {
//			transform.localRotation = temp;
//			flapWingsAnim.SetTrigger (flapLeftHash);
//		} else {
//			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime);
//			flapWingsAnim.SetTrigger (flapRightHash);
//		}

	}

	public void RightFlap() {

		transform.Rotate (transform.forward, flapRotationStrength, Space.World);
		flapWingsAnim.SetTrigger (flapRightHash);

//		Quaternion zQuaternion = Quaternion.AngleAxis(flapRotationStrength, transform.forward);
//		Quaternion temp = transform.localRotation * zQuaternion;
//
//		// If the butterfly is on the ground, clamp its rotation. 
//		if (!is_Landed) {
//			transform.localRotation = temp;
//			flapWingsAnim.SetTrigger (flapRightHash);
//		} else if (Quaternion.Angle (center, temp) >= -45.0) {
//			transform.localRotation = temp;
//			flapWingsAnim.SetTrigger (flapRightHash);
//		} else {
//			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime);
//			flapWingsAnim.SetTrigger (flapLeftHash);
//		}
	}

	public void BothFlap() {
		//rb = GetComponent<Rigidbody> ();
		//		print (both_flap_active);
		//		rb.AddForce (transform.forward * wingThrust / 2 + transform.up * wingThrust);
		if (both_flap_active == false) {

			if (movementSpeed.z < maxVelocities.z) {
				movementSpeed.z += wingThrust / 2;
			}
			if (movementSpeed.y < maxVelocities.y) {
				movementSpeed.y += wingThrust / 2;
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
			maxVelocities.z = 5.0f;
			movementSpeed.x = 0.0f;
			movementSpeed.z = 0.0f;
			movementSpeed.y = 0.0f;
			is_Landed = true;
		} else {
			if (is_Landed == true && terrainHeightPlaneLocale + 5 <= transform.position.y) {
				maxVelocities.z = 400.0f;
				maxVelocities.y = 50.0f;
				is_Landed = false;
				print (this.gameObject.name + " is_Landed = " + is_Landed);
			}
			if (terrainHeightPlaneLocale + 3 < transform.position.y) {
				movementSpeed -= transform.InverseTransformDirection(Vector3.up) * gravity * Time.deltaTime;
				lift = transform.TransformDirection (transform.up).y * (movementZSpeed / 150);
				if (movementSpeed.y < 50.0f) {
					movementSpeed.y += lift * gravity * Time.deltaTime;
				}
			}
			rb.MovePosition(transform.position + (transform.up * movementSpeed.y * Time.fixedDeltaTime) + (transform.forward * movementSpeed.z * Time.fixedDeltaTime) + (transform.right * movementXSpeed * Time.fixedDeltaTime));
			if (rb.position.y < 1) {
				rb.position = new Vector3 (rb.position.x, 1, rb.position.z);
			}

			// Yaw based on how we're rolled.
			float tip = (transform.right + Vector3.up).magnitude - 1.414214f;
			yaw -= tip;
		}
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
