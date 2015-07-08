using UnityEngine;
using System.Collections;

public class Hittable : MonoBehaviour {

	public float hitPoints = 100f;
	public bool isBig = false;
	public int points = 0;
	public float defconPoints = 0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.tag == "Bullet" && (collider.gameObject.GetComponent<Suckable>() == null || !collider.gameObject.GetComponent<Suckable>().beingSucked)) {
			if(GetComponent<Helicopter>() != null)// && collider.gameObject.GetComponentInChildren<JunkBullet>() == null)
				return;
			hitPoints -= collider.gameObject.GetComponent<Bullet>().damage;
			GameUI.UpdateUI();
			Instantiate(collider.gameObject.GetComponent<Bullet>().explosion, collider.transform.position, Quaternion.identity);
			if(isBig) {
				if(collider.gameObject.GetComponent<Bullet>().damage < 1f) {
					MCamera.ScreenShake(.15f, .5f);
				} else {
					MCamera.ScreenShake(.3f, 1f);
				}
				if(collider.gameObject.GetComponentInChildren<JunkBullet>() != null) {
					collider.gameObject.GetComponentInChildren<JunkBullet>().numhits -= 1;
					if(collider.gameObject.GetComponentInChildren<JunkBullet>().numhits <= 0)
						Destroy (collider.gameObject);
				} else {
					Destroy (collider.gameObject);
				}
			}
			if(hitPoints <= 0f){
				if(collider.gameObject.GetComponentInChildren<JunkBullet>() != null) {
					GameEngine.instance.score += points;
					GameEngine.instance.getDefconPoints(defconPoints);
				}
				if(gameObject.tag == "Roomba") {
					//Debug.Log("game over");
					//GameEngine.instance.Roombacalypse();
					GameEngine.instance.playerDefeated();
					//return;
				}
				if(GetComponentInParent<Tank>() != null){
					Instantiate (GetComponentInParent<Tank>().crater, transform.position, Quaternion.identity);
					Destroy (transform.parent.gameObject);
				} else {
					if(GetComponent<Civilian>() != null) {
						GetComponent<Civilian>().SplatterBlood();
					}
					Destroy (transform.gameObject);
				}
			} 
		}
	}
}
