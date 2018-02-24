using UnityEngine;

public class KeyCombo {

	public string[] buttons;
	private int currentIndex = 0; //moves along the array as buttons are pressed

	public float allowedTimeBetweenButtons = 0.1f; //tweak as needed
	private float timeLastButtonPressed;

	public KeyCombo(string[] b)
	{
		buttons = b;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//usage: call this once a frame. when the combo has been completed, it will return true
	public bool Check()
	{
		if (Time.time > timeLastButtonPressed + allowedTimeBetweenButtons) currentIndex = 0;
		{
			if (currentIndex < buttons.Length)
			{
				if ((buttons[currentIndex] == "down" && Input.GetAxisRaw("Vertical") == -1) ||
					(buttons[currentIndex] == "up" && Input.GetAxisRaw("Vertical") == 1) ||
					(buttons[currentIndex] == "left" && Input.GetAxisRaw("Vertical") == -1) ||
					(buttons[currentIndex] == "right" && Input.GetAxisRaw("Horizontal") == 1) ||
					(buttons[currentIndex] != "down" && buttons[currentIndex] != "up" && buttons[currentIndex] != "left" && buttons[currentIndex] != "right" && Input.GetButtonDown(buttons[currentIndex])))
				{
					timeLastButtonPressed = Time.time;
					currentIndex++;
				}

				if (currentIndex >= buttons.Length)
				{
					currentIndex = 0;
					return true;
				}
				else return false;
			}
		}

		return false;
	}
}
