using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

	private GameObject butterfly_P1;
	private GameObject butterfly_P2;
	private GameObject butterfly_P3;
	private GameObject butterfly_P4;
//	private GameObject camera_P1;
//	private GameObject camera_P2;
//	private GameObject camera_P3;
//	private GameObject camera_P4;
	private GameObject minimap;
	private List<GameObject> butterflies;
	private float mapScale = 500.0f / 262.5f;

	// Use this for initialization
	void Start () {
		butterfly_P1 = GameObject.Find ("butterfly_P1");
		butterfly_P2 = GameObject.Find ("butterfly_P2");
		butterfly_P3 = GameObject.Find ("butterfly_P3");
		butterfly_P4 = GameObject.Find ("butterfly_P4");
		minimap = GameObject.Find ("MiniMap Container");
//		camera_P1 = GameObject.Find("camera_P1");
//		camera_P2 = GameObject.Find("camera_P2");
//		camera_P3 = GameObject.Find("camera_P3");
//		camera_P4 = GameObject.Find("camera_P4");
		butterflies = new List<GameObject> {butterfly_P1, butterfly_P2, butterfly_P3, butterfly_P4};
//		print ("Found butterfly_P1 with name " + butterfly_P1.name);
//		print ("Found butterfly_P2 with name " + butterfly_P2.name);
//		print ("Found butterfly_P3 with name " + butterfly_P3.name);
//		print ("Found butterfly_P4 with name " + butterfly_P4.name);
//		print ("Found camera_P1 with name " + camera_P1.name);
//		print ("Found camera_P2 with name " + camera_P2.name);
//		print ("Found camera_P3 with name " + camera_P3.name);
//		print ("Found camera_P4 with name " + camera_P4.name);
	}
	
	// Update is called once per frame
	void Update () {
		displayMinimap ();
	}

	void displayMinimap() {
		for (int i = 0; i < GlobalController.numberOfPlayers; i++ ) {
			float worldXPos = butterflies[i].transform.position.x;
			float worldZPos = butterflies[i].transform.position.z;
			float xPos = (worldXPos - 5000) / 10;
			float yPos = (worldZPos - 5000) / 10;
//			if (butterfly.name == "butterfly_P1") print ("xPos = " + xPos + ", yPos = " + yPos);
			string player = butterflies[i].name.Substring (butterflies[i].name.Length - 2);
			minimap.transform.Find (player).localPosition = new Vector3 (xPos, yPos, 0);
		}
	}
}
