using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyControlPhysics : MonoBehaviour {

	Vector3 vel = new Vector3(0, 0, 0);

	public Vector3 gravity = new Vector3(0, -6, 0);
	//Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
		//rb = GetComponent<Rigidbody> ();
	}

	/*
	// Update is called once per frame
	void Update () {
		
	}
	*/

	public void leftFlap() {

	}

	public void rightFlap() {

	}

	public void bothFlap() {
		vel += transform.TransformDirection(new Vector3 (0, 1, 0)) * 3.7f;
	}

	public void advance() {

		vel += gravity * Time.deltaTime;

		transform.position += vel;
	}

	public void reorientateUp() {
		transform.up = new Vector3 (0, 1, 0);
	}
}
