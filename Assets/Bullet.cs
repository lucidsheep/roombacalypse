using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	public float maxSpeed = 10f;
	public float acceleration = 10f;
	public float damage = 0f;

	public GameObject explosion;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.magnitude > 60f) {
			Destroy (this.gameObject);
		}
	}

	void FixedUpdate() {
		float angle = transform.eulerAngles.magnitude * Mathf.Deg2Rad;
		Vector2 force = new Vector2(acceleration * Mathf.Cos (angle), acceleration * Mathf.Sin (angle));
		//transform.TransformDirection(Vector3.forward);
		GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
		
		if(GetComponent<Rigidbody2D>().velocity.magnitude > maxSpeed) {
			GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, maxSpeed);
		}
	}
}
