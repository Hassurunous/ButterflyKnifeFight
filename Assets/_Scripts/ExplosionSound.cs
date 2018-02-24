using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class ExplosionSound : MonoBehaviour 
{
	public AudioSource explosion1;
	public AudioSource explosion2;
	public AudioSource explosion3;
	public AudioSource explosion4;
	public AudioSource explosion5;
	public AudioSource explosion6;

	// Use this for initialization
	void Start () 
	{
		playRandomExplosion ();
	}

	void playRandomExplosion()
	{
		int randSong = Random.Range (1, 6);

		switch (randSong) 
		{
		case 1:
			explosion1.Play();
			break;
		case 2:
			explosion2.Play();
			break;
		case 3:
			explosion3.Play();
			break;
		case 4:
			explosion4.Play();
			break;
		case 5:
			explosion5.Play();
			break;
		case 6:
			explosion6.Play();
			break;
		default:
			explosion1.Play ();
			break;
		}
	}
}
