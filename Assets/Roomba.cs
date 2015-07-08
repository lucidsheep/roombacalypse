using UnityEngine;
using System.Collections;

public class Roomba : MonoBehaviour {

	public float maxSpeed = 50f;
	public float maxSpeedSucking = 10f;
	public float acceleration = 5f;
	public float turnAngle = 45f;
	public int suckDamage = 50;

	public Sprite normalState;
	public Sprite suckState;
	public Sprite fullState;
	public Sprite damagedState;

	public static Roomba instance;
	void Awake() {
		instance = this;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(GetComponent<Hittable>().hitPoints <= 3) {
			setState(damagedState);
		} else if(GetComponentInChildren<Vacuum>().sucking) {
			setState(suckState);
		} else if(GetComponentInChildren<Vacuum>().fill >= GetComponentInChildren<Vacuum>().capacity) {
			setState(fullState);
		} else {
			setState(normalState);
		}
		/*
		if(Input.GetKey(KeyCode.D)) {
			Rotate(0);
		} else if(Input.GetKey(KeyCode.A)) {
			Rotate(1);
		}
		*/
	}

	void setState(Sprite state){
		GetComponent<SpriteRenderer>().sprite = state;
	}
	void Rotate(int direction) {
		float angle = transform.rotation.eulerAngles.z;
		if(direction == 0) {
			angle -= (turnAngle * 60F) * Time.deltaTime;
		} else {
			angle += (turnAngle * 60F) * Time.deltaTime;
		}
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = q;
	}
	void FixedUpdate() {
		Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

		float angle = transform.eulerAngles.magnitude * Mathf.Deg2Rad;
		Vector2 force = new Vector2(acceleration * Mathf.Cos (angle), acceleration * Mathf.Sin (angle));
		//transform.TransformDirection(Vector3.forward);
		GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
		float cap = maxSpeed;
		if(GetComponentInChildren<Vacuum>().sucking)
			cap = maxSpeedSucking;
		if(GetComponent<Rigidbody2D>().velocity.magnitude > cap) {
			GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, cap);
		}
	}
}
