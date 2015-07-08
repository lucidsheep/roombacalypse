using UnityEngine;
using System.Collections;

public class CentralAudio : MonoBehaviour {

	public AudioClip splat;
	public AudioClip filled;
	public AudioClip bigExplosion;

	public static CentralAudio instance;
	// Use this for initialization
	void Awake() {
		instance = this;
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playClip(AudioClip clip) {
		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().clip = clip;
		GetComponent<AudioSource>().loop = false;
		GetComponent<AudioSource>().Play();
	}
}
