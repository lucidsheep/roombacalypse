using UnityEngine;
using System.Collections;

public class Vacuum : MonoBehaviour {

	public static bool touchInput = true;
	public static int prevTouches = 0;
	public static float lastTouchCooldown = 0f;
	public static bool doubleTap = false;
	public GameObject bullet;
	public AudioClip suckSFX;
	public AudioClip shotSFX;
	public AudioClip filledSFX;
	public int capacity = 100;
	public int fill = 0;
	public bool sucking = false; // lol
	
	// Use this for initialization
	void Start () {
		setSucking(false);
		if(Input.touchSupported)
			touchInput = true;
		touchInput = true; //hack
	}

	void updateTouchController() {
		lastTouchCooldown -= Time.deltaTime;
		doubleTap = false;
		if(Input.GetMouseButtonDown(0)) {
			if(lastTouchCooldown > 0f) {
				doubleTap = true;
				lastTouchCooldown = 0f;
			} else {
				lastTouchCooldown = .20f;
			}
		}
		/*
		if(Input.GetMouseButtonDown(0) && lastTouchCooldown > 0f) {
			doubleTap = true;
			lastTouchCooldown = 0f;
		}
		*/
		prevTouches = 0;
	}
	// Update is called once per frame
	void Update () {
		if(touchInput)
			updateTouchController();
		if(fill < capacity) {
			if(touchInput){
				if(doubleTap) {
					//mousedown
					setSucking(true);
				} else if(Input.GetMouseButton(0) == false) {
					//mouseup
					setSucking(false);
				}
			} else {
				if(Input.GetKey(KeyCode.S) || Input.GetMouseButtonDown(0)) {
					setSucking(true);

				} else if(Input.GetMouseButtonUp(0)){
					setSucking(false);
				}
			}
		} else {
			if(touchInput) {
				if(doubleTap) {
					//mouedown
					shootJunk();
				}
			} else {
				if(Input.GetKeyDown(KeyCode.S) || Input.GetMouseButtonDown(0)) {
					shootJunk();
				}
			}
		}
		if(sucking) {
			MCamera.ScreenShake(.2f, .25f);
		}
		prevTouches = Input.touchCount;
	}

	void shootJunk(){
		GameObject newBullet = (GameObject)Instantiate(bullet, GetComponentInParent<Roomba>().transform.position, GetComponentInParent<Roomba>().transform.rotation);
		Physics2D.IgnoreCollision(newBullet.GetComponent<Collider2D>(), GetComponentInParent<CircleCollider2D>());
		//newBullet.transform.localScale = new Vector3(.02f * fill, .02f * fill, 1f);
		newBullet.GetComponent<Bullet>().damage = 1f;
		if(fill > 2000)
			newBullet.GetComponent<Bullet>().damage = 2f;
		newBullet.GetComponentInChildren<JunkBullet>().setSize(fill);
		fill = 0;
		GameUI.UpdateUI();

		GetComponent<AudioSource>().clip = shotSFX;
		GetComponent<AudioSource>().loop = false;
		GetComponent<AudioSource>().Play();
	}
	public void suckObject(Suckable obj) {
		if(fill >= capacity) {
			return;
		}
		fill += obj.size;
		GameUI.UpdateUI();
		if(fill >= capacity) {
			//fill = capacity;
			//GetComponent<AudioSource>().Stop();
			CentralAudio.instance.playClip(CentralAudio.instance.filled);
			/*
			GetComponent<AudioSource>().clip = filledSFX;
			GetComponent<AudioSource>().loop = true;
			GetComponent<AudioSource>().Play();
			*/
			setSucking(false);
		}
	}

	void setSucking(bool state){
		sucking = state;
		if(sucking) {
			transform.localPosition = new Vector3(1.91f, 0f, 0f);
			GetComponent<Collider2D>().transform.localScale = new Vector3(1f, 1f, 1f);
			GetComponentInChildren<SpriteRenderer>().enabled = true;
			GetComponent<AudioSource>().clip = suckSFX;
			GetComponent<AudioSource>().loop = true;
			GetComponent<AudioSource>().Play();
		} else {
			transform.localPosition = new Vector3(-1000f, -1000f, 0f);
			GetComponent<Collider2D>().transform.localScale = new Vector3(.01f, .01f, 1f);
			GetComponentInChildren<SpriteRenderer>().enabled = false;
			GetComponent<AudioSource>().Stop ();
		}
	}
	void OnTriggerEnter2D(Collider2D collider) {
		//Debug.Log ("trigger enter");
		if(sucking && collider.gameObject.GetComponent<Suckable>() != null) {
			collider.gameObject.GetComponent<Suckable>().beingSucked = true;
		}
	}

	
	void OnTriggerExit2D(Collider2D collider) {
		if(collider.gameObject.GetComponent<Suckable>() != null) {
			collider.gameObject.GetComponent<Suckable>().beingSucked = false;
		}
	}
}
