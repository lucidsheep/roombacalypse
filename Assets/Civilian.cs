using UnityEngine;
using System.Collections;

public class Civilian : MonoBehaviour {
	public float minSpeed;
	public float maxSpeed;
	public GameObject blood;
	public AudioClip scream;
	public AudioClip squish;

	float speed;
	int animType;
	bool panicking = false;
	// Use this for initialization
	void Start () {
		speed = Random.Range (minSpeed, maxSpeed);
		animType = Random.Range(1, 5);
		if(animType == 1)
			GetComponent<Animator>().Play("ZombieAnim"); //= GetComponent<Animation>().GetClip("ZombieAnim");
		else if(animType == 2)
			GetComponent<Animator>().Play ("ZombieAnim2");
		else if(animType == 3)
			GetComponent<Animator>().Play ("ZombieAnim3");
		else
			GetComponent<Animator>().Play ("ZombieAnim4");

	}
	
	// Update is called once per frame
	void Update () {
		if(GameEngine.gameOver)
			return;
		if(GetComponent<Suckable>().beingSucked) {
			float suckSpeed = 1f;
			if(GetComponent<Suckable>().hitPoints <= 0f)
				suckSpeed = 10f;
			Vector2 moveVector = Vector2.MoveTowards(transform.position, Roomba.instance.transform.position, suckSpeed * Time.deltaTime);
			transform.position = moveVector;
			//GetComponent<Rigidbody2D>().velocity = moveVector;
		} else {
			if(Vector2.Distance(transform.position,Roomba.instance.transform.position) < 8f) {
				if(!panicking) {
					panicking = true;
					/*
					GetComponent<AudioSource>().Stop();
					GetComponent<AudioSource>().clip = scream;
					GetComponent<AudioSource>().loop = false;
					GetComponent<AudioSource>().Play();
					*/
				}
				Vector2 origin = new Vector2(transform.position.x, transform.position.y);
				Vector2 destination = new Vector2(Roomba.instance.transform.position.x, Roomba.instance.transform.position.y);
				Vector2 moveVector = Vector2.MoveTowards(origin, destination, speed * Time.deltaTime) * 1;
				Vector2 diff = origin - moveVector;
				Vector2 final = origin + diff;
				transform.position = final;

				//Vector3 diff = Roomba.instance.transform.position - GetComponentInParent<Tank>().transform.position;
				diff.Normalize();
				float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

			} else {
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				panicking = false;
			}
		}
	}

	public void SplatterBlood(){
		float range = 3f;
		for(int i = Random.Range(0, 6); i < 15; i++) {
			Vector3 variance = new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0f);
			Instantiate(blood, transform.position + variance, Quaternion.identity);
		}
	}
	void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.GetComponent<Civilian>() == null && GetComponent<Suckable>().beingSucked == false) {
			SplatterBlood();
			//GetComponent<AudioSource>().Stop();
			CentralAudio.instance.playClip(CentralAudio.instance.splat);
			if(collision.gameObject.GetComponent<Roomba>() != null) {
				GameEngine.instance.score += GetComponent<Hittable>().points;
				GameEngine.instance.getDefconPoints(GetComponent<Hittable>().defconPoints);
			}
			Destroy (this.gameObject);
		}
	}


	
}
