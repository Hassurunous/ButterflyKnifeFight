﻿using UnityEngine;
using System.Collections;

public class DontGoThroughThings : MonoBehaviour
{
	// Careful when setting this to true - it might cause double
	// events to be fired - but it won't pass through the trigger
	public bool sendTriggerMessage = false; 	

	public LayerMask layerMask = -1; //make sure we aren't in this layer 
	public float skinWidth = 0.1f; //probably doesn't need to be changed 

	private float minimumExtent; 
	private float partialExtent; 
	private float sqrMinimumExtent; 
	private Vector3 previousPosition; 
	private Rigidbody myRigidbody;
	private Collider myCollider;

	//initialize values 
	void Start() 
	{ 
		myRigidbody = GetComponent<Rigidbody>();
		myCollider = GetComponent<Collider>();
		previousPosition = myRigidbody.position; 
		minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z); 
		partialExtent = minimumExtent * (1.0f - skinWidth); 
		sqrMinimumExtent = minimumExtent * minimumExtent; 
	} 

	void FixedUpdate() 
	{ 
		//have we moved more than our minimum extent? 
		Vector3 movementThisStep = myRigidbody.position - previousPosition; 
		float movementSqrMagnitude = movementThisStep.sqrMagnitude;

		if (movementSqrMagnitude > sqrMinimumExtent) 
		{ 
			float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
			RaycastHit hitInfo; 

			//check for obstructions we might have missed 
			if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
			{
				if (!hitInfo.collider)
					return;

				if (!hitInfo.collider.isTrigger) {
					print ("Detected imminent collision in next movement step. Collision with " + hitInfo.collider.name);
					myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent; 
					myRigidbody.GetComponent<ButterflyControlsv031> ().movementSpeed = new Vector3 (0.0f, 1.0f, 0.0f);
				}

			}
		} 

		previousPosition = myRigidbody.position; 
	}
}