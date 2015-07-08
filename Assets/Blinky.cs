using UnityEngine;
using System.Collections;

public class Blinky : MonoBehaviour {

	float blinkCycle;
	bool animating = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!animating && Roomba.instance.GetComponentInChildren<Vacuum>().fill >= Roomba.instance.GetComponentInChildren<Vacuum>().capacity) {
			animating = true;
			blinkCycle = 2f;
		} else if(animating && Roomba.instance.GetComponentInChildren<Vacuum>().fill < Roomba.instance.GetComponentInChildren<Vacuum>().capacity) {
			animating = false;
		}
		Color curColor = GetComponent<SpriteRenderer>().color;
		if(animating) {
			blinkCycle -= Time.deltaTime * 3f;
			if(blinkCycle > 1f) {
				float range = (blinkCycle - 1f);
				GetComponent<SpriteRenderer>().color = new Color(curColor.r, curColor.g, curColor.b, range);
			} else {
				float range = 1f - blinkCycle;
				GetComponent<SpriteRenderer>().color = new Color(curColor.r, curColor.g, curColor.b, range);
			}
			if(blinkCycle <= 0f) {
				blinkCycle += 2f;
			}
		} else {
			GetComponent<SpriteRenderer>().color = new Color(curColor.r, curColor.g, curColor.b, 0f);
		}
	}
}
