using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour {

	public float bias = 0.85f;
	public float followDistance = 10.0f;
	public float followHeight = 2.0f;
	public float cameraVertOffsetMax = 5.0f;
	public float cameraVertOffsetMin = 3.0f;
	public float cameraFollowOffsetMax = 45.0f;
	public float cameraFollowOffsetMin = 30.0f;
	public GameObject butterfly;
	public Camera camera;

	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		if (camera != null && butterfly != null) {
			if (followDistance < cameraFollowOffsetMin + 1) {
				followDistance = cameraFollowOffsetMin;
			} else if (followDistance <= cameraFollowOffsetMax) {
				followDistance = followDistance * bias +  butterfly.GetComponent<ButterflyControlsv03>().movementZSpeed * (1.0f - bias);
			} else {
				followDistance = cameraFollowOffsetMax;
			}
			if (followHeight < cameraVertOffsetMin + 1) {
				followHeight = cameraVertOffsetMin;
			} else if (followHeight <= cameraVertOffsetMax) {
				followHeight = followHeight * bias - butterfly.GetComponent<ButterflyControlsv03>().movementZSpeed * (1.0f - bias);
			} else {
				followHeight = cameraVertOffsetMax;
			}
			//		print ("followHeight = " + followHeight);
			//		print ("followDistance = " + followDistance);
			Vector3 moveCamTo = transform.position + transform.up * followHeight + -transform.forward * followDistance;
			camera.transform.position = camera.transform.position * bias + moveCamTo * (1.0f - bias);
			camera.transform.LookAt( transform.position + transform.forward * followDistance);
		}
	}
}
