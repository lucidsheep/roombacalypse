using UnityEngine;
using System.Collections;

public class Suckable : MonoBehaviour {

	public float hitPoints = 100;
	public float defconPoints = 0;
	public int size = 10;
	public int points = 0;
	public bool beingSucked = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(GameEngine.gameOver)
			return;
		if(beingSucked) {
			hitPoints -= Roomba.instance.suckDamage * Time.deltaTime;
			if(hitPoints <= 0 && Physics2D.IsTouching(Roomba.instance.GetComponent<CircleCollider2D>(), GetComponent<Collider2D>())) {
				getSucked();
			}
		}
	}
	void getSucked() {
		GameEngine.instance.score += points;
		GameEngine.instance.getDefconPoints(defconPoints);
		Roomba.instance.GetComponentInChildren<Vacuum>().suckObject(this);
		if(GetComponentInParent<Tank>() != null)
			Destroy (transform.parent.gameObject);
		else
			Destroy (this.gameObject);
	}
	void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag == "Roomba") {
			if(hitPoints <= 0f) {
				getSucked();
				/*
				if(GetComponentInParent<Transform>() != null) {
					Destroy (GetComponentInParent<Transform>().gameObject);
				} else {
					Destroy (this.gameObject);
				}
				*/
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.tag == "Roomba") {
			if(hitPoints <= 0f) {
				getSucked();
			}
		}
	}
	/*
	void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.tag == "Roomba") {
			if(hitPoints <= 0f) {
				collision.gameObject.GetComponentInChildren<Vacuum>().suckObject(this);
				Destroy (this.gameObject);
			}
		}
	}
	*/
}
