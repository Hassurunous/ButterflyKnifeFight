	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameController: MonoBehaviour {

	// Variables
	public static GameController instance = null;
	public GameObject knife;
	public Camera camera;
	public GameObject butterfly;
	public int numberOfPlayers;
	private int knifeCounter;
	private int killsToWin = 3;
	private List<GameObject> deadButterflies = new List<GameObject> ();
	private float spawnCounter = 0.0f;
	private List<float> spawnTimes = new List<float> ();
	private bool butterflyDied;
	private string warningMsgName;
	public GameObject UICanvas;
	private GameObject EndGameOverlay;
	private GameObject PauseAndEndText;
	private GameObject MiniMapContainer;
	private GameObject MainMenuButton;
	private GameObject RestartGameButton;
	private GameObject PlayAgainButton;
	private GameObject globalController;
	public bool gamePaused = false;
	private string winnerName;
	public Texture[] wings; 

	// Respawn points
	List<Vector3> reSpawnPoints = new List<Vector3> {
		new Vector3(4350, 200, 4350),
		new Vector3(5650, 200, 4350),
		new Vector3(5650, 200, 5650),
		new Vector3(4350, 200, 5650),
		new Vector3(4650, 200, 4650),
		new Vector3(5350, 200, 4650),
		new Vector3(5350, 200, 5350),
		new Vector3(4650, 200, 5350),
		new Vector3(3687, 200, 5000),
		new Vector3(5000, 200, 3687),
		new Vector3(6313, 200, 5000),
		new Vector3(5000, 200, 6313)
	};

	public Color[] playerColors = {
		new Color (1.0f, 0.0f, 0.0f, 1.0f),
		new Color (0.0f, 0.8f, 1.0f, 1.0f),
		new Color (1.0f, 1.0f, 0.0f, 1.0f),
		new Color (0.0f, 1.0f, 0.0f, 1.0f)
	};

	// Spawn point pointer to cycle spawn points when players die
	public int spawnPointer = 0;

	// Enumeration of state that the game can be on
	public enum GameState {Spawn, Playing, GameOver}

	// The current game state
	public GameState currentState;

	// Time since last state change happened
	float lastStateChange = 0.0f;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		switch (GlobalController.numberOfPlayers) {
		case 2:
			UICanvas.transform.GetChild (0).gameObject.SetActive (true);
			break;
		case 3:
			UICanvas.transform.GetChild (1).gameObject.SetActive (true);
			break;
		case 4:
			UICanvas.transform.GetChild (2).gameObject.SetActive (true);
			break;
		}

		knifeCounter = Random.Range(15, 20);
		makeItRain (50);
//		globalController = GlobalController.gameObject;

		numberOfPlayers = GlobalController.numberOfPlayers;

		spawnPlayers();
		setCurrentState (GameState.Spawn);

		EndGameOverlay = UICanvas.transform.Find ("EndGameOverlay").gameObject;
		PauseAndEndText = UICanvas.transform.Find ("Pause and End Text").gameObject;
		MiniMapContainer = UICanvas.transform.Find ("MiniMap Container").gameObject;
		MainMenuButton = UICanvas.transform.Find ("Main Menu Button").gameObject;
		PlayAgainButton = UICanvas.transform.Find ("Play Again Button").gameObject;
		RestartGameButton = UICanvas.transform.Find ("Restart Button").gameObject;
		MainMenuButton.GetComponent<Button> ().onClick.AddListener(() => GlobalController.mainMenu());
		RestartGameButton.GetComponent<Button> ().onClick.AddListener(() => GlobalController.startGame());
		PlayAgainButton.GetComponent<Button> ().onClick.AddListener(() => GlobalController.startGame());
	}

	// Set current state and last state change time
	void setCurrentState(GameState state) {
		currentState = state;
		lastStateChange = Time.time;
	}

	// Get time difference since last state change
	float GetElapsedTime() {
		return Time.time - lastStateChange;
	}

	// Spawn knife from the sky on a random x and z positions
	private void spawnKnife() {
		Vector3 spawnPosition = new Vector3 (Random.Range(3900, 6100), 1200, Random.Range(3900, 6100));
		Quaternion spawnRotation = Quaternion.Euler(-180, 180, -90);
		GameObject clone = Instantiate (knife, spawnPosition, spawnRotation);
		clone.GetComponent<Rigidbody> ().AddForce (Vector3.down * 9001); // It's over 9000!!
	}

	// Spawn many knives from the sky
	private void makeItRain(int amount) {
		for (int i = 0; i < amount; i++) {
			spawnKnife ();
		}
	}

	// Get notification that player died from collision scripts
	public void playerDied(GameObject deadButterfly, float deathTime) {
		deadButterflies.Add (deadButterfly);
		spawnTimes.Add (deathTime);

		butterflyDied = true;
	}

	// Spawn butterflies and cameras
	public void spawnPlayers() {
		Quaternion spawnRotation = Quaternion.identity;
		Vector3 cameraSpawnPosition = new Vector3 (5000, 300, 5000);
		List<Vector3> spawnPoints = new List<Vector3> {
			new Vector3(4350, 200, 4350),
			new Vector3(5650, 200, 4350),
			new Vector3(5650, 200, 5650),
			new Vector3(4350, 200, 5650),
			new Vector3(4650, 200, 4650),
			new Vector3(5350, 200, 4650),
			new Vector3(5350, 200, 5350),
			new Vector3(4650, 200, 5350)
		};

		for (int i = 0; i < numberOfPlayers; i++) {
			int randI = Random.Range (0, spawnPoints.Count);
			int uiPlayers = numberOfPlayers - 2;
			string newButterflyName = "butterfly_P" + (i + 1).ToString ();
			string newCameraName = "camera_P" + (i + 1).ToString ();
			string warningMsgName = "Warning_P" + (i + 1).ToString ();

			Camera cameraClone = Instantiate (camera, cameraSpawnPosition, spawnRotation);
			GameObject butterflyClone = Instantiate (butterfly, spawnPoints[randI], spawnRotation);

			butterflyClone.gameObject.name = newButterflyName;

			butterflyClone.GetComponent<ButterflyControlsv031> ().horizontalAxis = "HorizontalP" + (i + 1).ToString ();
			butterflyClone.GetComponent<ButterflyControlsv031> ().verticalAxis = "VerticalP" + (i + 1).ToString ();

			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) {
				

				butterflyClone.GetComponent<ButterflyControlsv031> ().flapLAxis = "MacFlapLP" + (i + 1).ToString (); 
				butterflyClone.GetComponent<ButterflyControlsv031> ().flapRAxis = "MacFlapRP" + (i + 1).ToString (); 
				butterflyClone.GetComponent<ButterflyControlsv031> ().bombsAwayAxis = "MacBomb's AwayP" + (i + 1).ToString ();
				butterflyClone.GetComponent<ButterflyControlsv031> ().pauseButton = "MacPauseP" + (i + 1).ToString ();

			} else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) {
				
				butterflyClone.GetComponent<ButterflyControlsv031> ().flapLAxis = "WindowsFlapLP" + (i + 1).ToString (); 
				butterflyClone.GetComponent<ButterflyControlsv031> ().flapRAxis = "WindowsFlapRP" + (i + 1).ToString (); 
				butterflyClone.GetComponent<ButterflyControlsv031> ().bombsAwayAxis = "WinLinBomb's AwayP" + (i + 1).ToString ();
				butterflyClone.GetComponent<ButterflyControlsv031> ().pauseButton = "WinLinPauseP" + (i + 1).ToString ();

			} else if (Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer) {
				
				butterflyClone.GetComponent<ButterflyControlsv031> ().flapLAxis = "LinuxFlapLP" + (i + 1).ToString (); 
				butterflyClone.GetComponent<ButterflyControlsv031> ().flapRAxis = "LinuxFlapRP" + (i + 1).ToString (); 
				butterflyClone.GetComponent<ButterflyControlsv031> ().bombsAwayAxis = "WinLinBomb's AwayP" + (i + 1).ToString ();
				butterflyClone.GetComponent<ButterflyControlsv031> ().pauseButton = "WinLinPauseP" + (i + 1).ToString ();

			}

//			Texture new_wing = wings [Random.Range (0, wings.Length)];

//			butterflyClone.transform.GetChild (0).GetChild (3).GetComponent<Renderer> ().material.SetTexture ("_MainTex", new_wing);
//			butterflyClone.transform.GetChild (0).GetChild (6).GetComponent<Renderer> ().material.SetTexture ("_MainTex", new_wing);

			cameraClone.gameObject.name = newCameraName;

//			print (warningMsgName);
//			print (UICanvas.transform.GetChild (uiPlayers).transform.Find (warningMsgName).GetComponent<Text> ());
			butterflyClone.GetComponent<ButterflyControlsv031> ().warningMsg = UICanvas.transform.GetChild(uiPlayers).transform.Find (warningMsgName).GetComponent<Text> ();

			cameraClone.rect = getCameraRect (i);

			Transform butterflyChild = butterflyClone.transform.Find("butterfly");
			Transform butterflyLWing = butterflyChild.transform.Find("L_wing");
			Transform butterflyRwing = butterflyChild.transform.Find("R_wing");
			Transform particleSystemL = butterflyLWing.transform.Find("wing_particles_L");
			Transform particleSystemR = butterflyRwing.transform.Find("wing_particles_R");
//			print ("Found butterfly parts. " + butterflyChild.name + " " + butterflyLWing.name + " " + butterflyRwing.name);

			Color playerColor;

			if (i < playerColors.Length)
				playerColor = playerColors [i];
			else
				playerColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);

			butterflyChild.GetComponent<Renderer> ().material.SetColor ("_OutlineColor", playerColor);
			butterflyLWing.GetComponent<Renderer> ().material.SetColor ("_OutlineColor", playerColor);
			butterflyRwing.GetComponent<Renderer> ().material.SetColor ("_OutlineColor", playerColor);
			particleSystemL.GetComponent<ParticleSystem>().startColor = playerColor;
			particleSystemR.GetComponent<ParticleSystem>().startColor = playerColor;


			CinemachineVirtualCamera cmvc = cameraClone.GetComponentInChildren<CinemachineVirtualCamera> ();
			cmvc.Follow = butterflyClone.transform;
			cmvc.LookAt = butterflyClone.transform;
			cmvc.gameObject.layer = 8 + i;

			string cullingMaskName = "Player " + (i + 1);
			cameraClone.cullingMask |= 1 << LayerMask.NameToLayer(cullingMaskName);


//			Transform cameraTracking = butterflyClone.transform.Find("CameraTracking");
//
//			cameraTracking.GetComponent<CameraTracking>().camera = cameraClone;
//			cameraTracking.GetComponent<CameraTracking>().butterfly = butterflyClone;
//			cameraTracking.GetComponent<CameraTracking>().enabled = true;

			spawnPoints.RemoveAt (randI);
		}
	}

	public void checkForGameEnd (int kills, string playerName) {
		if (kills >= killsToWin) {
			winnerName = playerName;
			setCurrentState (GameState.GameOver);
		}
	}

	private void finishGame() {
		EndGameOverlay.SetActive (true);
		PauseAndEndText.SetActive (true);
		PlayAgainButton.SetActive (true);
		MainMenuButton.SetActive (true);
		int playerNum = System.Int32.Parse( winnerName[11] + "" );
		switch (playerNum) {
		case 1:
			PauseAndEndText.GetComponent<Text> ().text = "Red Player Wins!";
			PauseAndEndText.GetComponent<Text> ().color = playerColors [0];
			break;
		case 2:
			PauseAndEndText.GetComponent<Text>().text = "Blue Player Wins!";
			PauseAndEndText.GetComponent<Text> ().color = playerColors [1];
			break;
		case 3:
			PauseAndEndText.GetComponent<Text>().text = "Yellow Player Wins!";
			PauseAndEndText.GetComponent<Text> ().color = playerColors [2];
			break;
		case 4:
			PauseAndEndText.GetComponent<Text>().text = "Green Player Wins!";
			PauseAndEndText.GetComponent<Text> ().color = playerColors [3];
			break;
		}
		MiniMapContainer.SetActive (false);
	}

	Rect getCameraRect(int index) {
		float x;
		float y;
		float width;
		float height = 0.5f;

		switch (index) {
		case 0:
			x = 0f;
			y = 0.5f;
			if (numberOfPlayers == 1) {
				y = 0;
				width = 1;
				height = 1;
			}
			else if (numberOfPlayers == 2 || numberOfPlayers == 3) { 
				width = 1f;
			} else {
				width = 0.5f;
			}
			break;
		case 1:
			if (numberOfPlayers == 2) {
				x = 0;
				y = 0;
				width = 1;
			} else if (numberOfPlayers == 3) {
				x = 0;
				y = 0;
				width = 0.5f;
			} else {
				x = 0.5f;
				y = 0.5f;
				width = 0.5f;
			}

			break;
		case 2:
			if (numberOfPlayers == 3) {
				x = 0.5f;
				y = 0;
			} else {
				x = 0;
				y = 0;
			}
			width = 0.5f;
			break;
		case 3:
			x = 0.5f;
			y = 0;
			width = 0.5f;
			break;
		default:
			x = 0;
			y = 0;
			width = 1;
			break;
		}

		return new Rect (x, y, width, height);
	}

	// Update is called once per frame
	void Update () {
		if (gamePaused == true) {
			Time.timeScale = 0;
			EndGameOverlay.SetActive (true);
			PauseAndEndText.SetActive (true);
			PauseAndEndText.GetComponent<Text> ().text = "Game Paused";
			RestartGameButton.SetActive (true);
			MainMenuButton.SetActive (true);
		} else {
			Time.timeScale = 1;
			EndGameOverlay.SetActive (false);
			PauseAndEndText.GetComponent<Text> ().text = "";
			PauseAndEndText.SetActive (false);
			RestartGameButton.SetActive (false);
			MainMenuButton.SetActive (false);
		}
		switch (currentState) {
		case GameState.Spawn:
			// Spawn butterflies on selected spawn points and switch to playing state

			setCurrentState (GameState.Playing);
			break;
		case GameState.Playing:
			// Playing state main logic goes here
			if (knifeCounter <= GetElapsedTime ()) {
				if (knifeCounter >= 30) {
					knifeCounter += Random.Range (1, 5);
				} else {
					knifeCounter += Random.Range (15, 20);
				}
				makeItRain (Random.Range(10, 50));
			}

			if (butterflyDied) {
				if (deadButterflies.Count == 0) {
					spawnTimes.Clear ();
					spawnCounter = 0;
					butterflyDied = false;
				}
				else if (Time.time >= spawnTimes[0] + 5) {
					// Reset butterfly values
					Vector3 randomPos = reSpawnPoints [spawnPointer];
					deadButterflies [0].transform.position = randomPos;
					deadButterflies [0].GetComponent<ButterflyControlsv031> ().movementSpeed = new Vector3(0.0f, 1.0f, 0.0f);
					// Respawn butterfly
					deadButterflies [0].SetActive (true);
					print(deadButterflies [0].transform.GetChild(0));
					deadButterflies [0].transform.rotation = Quaternion.identity;
					deadButterflies.RemoveAt (0);
					spawnTimes.RemoveAt (0);
					spawnPointer += 1;
					// Keeping track of spawn rotation
					if (spawnPointer >= reSpawnPoints.Count) {
						spawnPointer = 0;
					}
				}
			}
			break;
		case GameState.GameOver:
			// Game ended display winner or something
			Debug.Log ("Game has ended!", gameObject);
			if (winnerName != null) {
				finishGame ();
				Time.timeScale = 0;
			}
			break;
		}
	}

	public void startGame() {
		print ("Game Start!");
		gamePaused = false;
		SceneManager.LoadScene ("BKFstage");
	}

	public void mainMenu() {
		print ("Game Start!");
		gamePaused = false;
		SceneManager.LoadScene ("Menu");
	}
}
