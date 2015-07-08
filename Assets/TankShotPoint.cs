using UnityEngine;
using System.Collections;

public class TankShotPoint : MonoBehaviour {

	public int flashCooldown = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(flashCooldown > 0) {
			flashCooldown--;
			GetComponentInChildren<SpriteRenderer>().enabled = true;
		} else
			GetComponentInChildren<SpriteRenderer>().enabled = false;
	}
}
