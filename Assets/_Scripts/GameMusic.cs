using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class GameMusic : MonoBehaviour 
{
	public AudioSource song0;
	public AudioSource song1;
	public AudioSource song2;
	public AudioSource song3;
	public AudioSource song4;

	// Use this for initialization
	void Start () 
	{
		playRandomSong ();
	}

	void playRandomSong()
	{
		int randSong = Random.Range (0, 4);
		switch (randSong) 
		{
		case 0:
			song0.Play();
			break;
		case 1:
			song1.Play();
			break;
		case 2:
			song2.Play();
			break;
		case 3:
			song3.Play();
			break;
		case 4:
			song4.Play();
			break;
		default:
			song0.Play ();
			break;
		}
	}
}