using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	public GameObject[] playersButtons = new GameObject[3];
	public ColorBlock selected;
	public ColorBlock unselected;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void setNumberOfPlayers(int number) {


		GlobalController.numberOfPlayers = number;
		for (int i = 0; i < 3; i++) {
//			print (i);
			if (number - 2 == i) {
//				print (i + "Players selected");
				playersButtons [i].GetComponent<Button>().colors = selected;
			} else {
//				print (i + "Players unselected");
				playersButtons [i].GetComponent<Button>().colors = unselected;
			}
		}
	}
}
