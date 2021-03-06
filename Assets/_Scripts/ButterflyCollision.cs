﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButterflyCollision : MonoBehaviour {

	public float time = 7;
	public bool groundCollision = false;
	public bool airCollision = false;
	public bool outCollision = false;
	public GameObject deathsplosion;
	public bool text = false;
	private string warningMsgName;
	public GameObject UICanvas;
	public Text warningMsg;
	private int UIPlayerNum = GlobalController.numberOfPlayers - 2;
	private List<string> killMsgs = new List<string> { "Siiick!!", "Got 'eem", "Savage!", "Kill!", "Nailed it!", "Noice!" };

	void Start() {

		warningMsgName = "Warning" + transform.name.Substring (transform.name.Length - 3);

		UICanvas = GameObject.FindGameObjectWithTag("Overlay Canvas");

		switch (GlobalController.numberOfPlayers) {
		case 2:
			warningMsg = UICanvas.transform.GetChild (0).transform.Find (warningMsgName).GetComponent<Text> ();
			break;
		case 3:
			warningMsg = UICanvas.transform.GetChild(1).transform.Find (warningMsgName).GetComponent<Text> ();
			break;
		case 4:
			warningMsg = UICanvas.transform.GetChild(2).transform.Find (warningMsgName).GetComponent<Text> ();
			break;
		}

	}

	void Update() {
		if (airCollision == true || outCollision == true) {
			time -= Time.deltaTime;

			if (outCollision == true && time <= 5) { 
				warningMsg.text = "Out of bounds! \n" + Mathf.CeilToInt (time);
			} else if (airCollision == true && time <= 5) {
				warningMsg.text = "Too high! \n" + Mathf.CeilToInt (time);
			}

			if (time <= 0) {
				Vector3 spawnPosition = transform.position;
				gameObject.SetActive (false);
				Instantiate (deathsplosion, spawnPosition, transform.rotation * Quaternion.Euler (0, 90, 0));
				GameController.instance.playerDied (gameObject, Time.time);

				groundCollision = false;
				airCollision = false;
				outCollision = false;
			}
		} else if (groundCollision == true) {
			warningMsg.text = "Flap both wings to fly!";
		}
	}

	void OnTriggerExit(Collider target)
	{
		if (target.gameObject.tag.Equals ("BattleZone") == true) {
			outCollision = true;
			text = true;
			time = 7;

//			print ("Out of bounds collision");
		} 
		else {
			groundCollision = false;
			airCollision = false;
			text = false;
			time = 7;
			warningMsg.text = "";

		}
	}

	void OnTriggerEnter(Collider target) {
		if (target.gameObject.tag.Equals ("BattleZone") == true) {
			outCollision = false;
			warningMsg.text = "";
			text = false;
			time = 7;
		} 
		else if (target.gameObject.tag.Equals ("AirZone") == true) {
			if (airCollision == false) {
				airCollision = true;
				text = true;
			} 
		} 
		else if (target.gameObject.tag.Equals ("GroundZone") == true) {
			if (groundCollision == false) {
				groundCollision = true;
				text = true;
			} 
		} 
		else if (target.gameObject.tag.Equals ("Knife Handle") == true) {
			print ("Butterfly collided with " + target.gameObject.name);
			print ("Collision tag = " + target.gameObject.tag);
			print ("Knife Handle");
			GameObject lastOwner = target.gameObject.transform.parent.gameObject.GetComponent<knifeDecay> ().lastOwner;
			if (lastOwner != gameObject) {
				foreach (Transform child in gameObject.transform) {
					if (child.CompareTag ("Knife")) {
						if (child.gameObject.activeSelf == false) {
							if (target.gameObject.transform.parent.parent == null) {// check if the parent of the knife is a butterfly
								Destroy (target.gameObject.transform.parent.gameObject);
							} else if (target.gameObject.transform.parent.parent.CompareTag ("ButterflyPrefab")) {
								target.gameObject.transform.parent.gameObject.SetActive (false);
							}
							child.gameObject.SetActive (true);
						}
					}
				}
			}
		} else if (target.gameObject.tag.Equals ("Knife Blade") == true) {
			print ("Butterfly collided with " + target.gameObject.name);
			print ("Collision tag = " + target.gameObject.tag);
			print ("Knife Blade");
			GameObject lastOwner = target.gameObject.transform.parent.gameObject.GetComponent<knifeDecay> ().lastOwner;
			if (lastOwner != gameObject) {
				Vector3 spawnPosition = transform.position;
				gameObject.SetActive (false);
				Instantiate (deathsplosion, spawnPosition, transform.rotation * Quaternion.Euler (0, 90, 0));
				if (target.gameObject.transform.parent.gameObject.GetComponent<knifeDecay> ().lastOwner != null) { // check if the knife was thrown by another butterfly
					Transform killer = target.gameObject.transform.parent.gameObject.GetComponent<knifeDecay> ().lastOwner.transform;
					if (killer.name != gameObject.transform.name) {
						ButterflyControlsv031 killerScript = killer.GetComponent<ButterflyControlsv031> ();
						killerScript.killCount += 1;
						string killerMsgName = "Warning" + killer.name.Substring (killer.name.Length - 3);
						string killerScoreName = "Kills" + killer.name.Substring (killer.name.Length - 3);
						Text killerMsg = UICanvas.transform.GetChild (UIPlayerNum).transform.Find (killerMsgName).GetComponent<Text> ();
						Text killerScore = UICanvas.transform.GetChild (UIPlayerNum).transform.Find (killerScoreName).GetComponent<Text> ();
						killerMsg.text = killMsgs [Random.Range (0, killMsgs.Count)];
						killerScore.text = killerScript.killCount + "/3";
						killerScript.killMsgActive = true;
						killerScript.killTime = Time.time;

						GameController.instance.checkForGameEnd (killerScript.killCount, killer.name);
					}
				} else if (target.gameObject.transform.parent.parent != null) {// check if the parent of the knife is a butterfly
					Transform killer = target.gameObject.transform.parent.parent;
					ButterflyControlsv031 killerScript = killer.GetComponent<ButterflyControlsv031> ();
					killerScript.killCount += 1;
					string killerMsgName = "Warning" + killer.name.Substring (killer.name.Length - 3);
					string killerScoreName = "Kills" + killer.name.Substring (killer.name.Length - 3);
					Text killerMsg = UICanvas.transform.GetChild (UIPlayerNum).transform.Find (killerMsgName).GetComponent<Text> ();
					Text killerScore = UICanvas.transform.GetChild (UIPlayerNum).transform.Find (killerScoreName).GetComponent<Text> ();
					killerMsg.text = killMsgs [Random.Range (0, killMsgs.Count)];
					killerScore.text = killerScript.killCount + "/3";
					killerScript.killMsgActive = true;
					killerScript.killTime = Time.time;

					GameController.instance.checkForGameEnd (killerScript.killCount, killer.name);

					print ("Kill Count = " + target.gameObject.transform.parent.parent.GetComponent<ButterflyControlsv031> ().killCount);
				}
				GameController.instance.playerDied (gameObject, Time.time);
			}
		}
	}

	void butterflyCollided(Rect coordinates, string collisionType) {
		GUIStyle textStyle = new GUIStyle();
		textStyle.fontStyle = FontStyle.Bold;
		textStyle.fontSize = 15;
		textStyle.normal.textColor = Color.red;
		textStyle.alignment = TextAnchor.MiddleCenter;

		GUI.Label(coordinates, collisionType, textStyle);
	}

	public void resetText() {
		warningMsg.text = "";
	}
}
