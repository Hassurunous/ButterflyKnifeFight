using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalController : MonoBehaviour {

	public static GlobalController instance = null;
	public static int numberOfPlayers = 4;


	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	public static void startGame() {
		print ("Game Start!");
		SceneManager.LoadScene ("BKFstage");
	}

	public static void mainMenu() {
		print ("Main Menu");
		SceneManager.LoadScene ("Menu");
	}

	public void quitGame() {
		print ("Quitting game...");
		Application.Quit ();
	}
}
