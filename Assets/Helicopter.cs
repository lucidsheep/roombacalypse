using UnityEngine;
using System.Collections;

public class Helicopter : MonoBehaviour {

	public float maxSpeed = 3f;
	public float acceleration = 3f;
	public float turnSpeed = .01f;
	public float shotTime = 5f;
	public GameObject bullet;
	float shotCooldown;
	// Use this for initialization
	void Start () {
		shotCooldown = shotTime;
	}

	void fireShot() {
		if(GameEngine.gameOver || GetComponent<Suckable>().beingSucked)
			return;
		Vector3 diff = Roomba.instance.transform.position - transform.position;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		
		GameObject newBullet = (GameObject)Instantiate(bullet, transform.position, Quaternion.identity);
		Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), newBullet.GetComponent<Collider2D>());
		newBullet.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
		newBullet.transform.rotation = transform.rotation;
	}

	// Update is called once per frame
	void Update () {
		if(GameEngine.gameOver)
			return;
		shotCooldown -= Time.deltaTime;
		if(shotCooldown <= 0f) {
			fireShot();
			shotCooldown = shotTime;
		}
		float curRot = transform.rotation.eulerAngles.z;
		Vector3 diff = Roomba.instance.transform.position - transform.position;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		if(GetComponent<Suckable>().beingSucked) 
			rot_z = curRot + 45f;
		if(Mathf.Abs(rot_z - curRot) > turnSpeed) {
			if(rot_z < 0f)
				rot_z += 360f;
			if(curRot < 0f)
				curRot += 360f;
			if(rot_z > curRot)
				rot_z = curRot + turnSpeed;
			else
				rot_z = curRot - turnSpeed;
		}

		if(GetComponent<Suckable>().beingSucked) 
			rot_z = curRot + 30f;

		transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
		if(GetComponent<Suckable>().beingSucked){
			if(GetComponentInChildren<Suckable>().beingSucked) {
				float suckSpeed = 1f;
				if(GetComponentInChildren<Suckable>().hitPoints <= 0f)
					suckSpeed = 10f;
				Vector2 moveVector = Vector2.MoveTowards(transform.position, Roomba.instance.transform.position, suckSpeed * Time.deltaTime);
				transform.position = moveVector;
				//GetComponent<Rigidbody2D>().velocity = moveVector;
			}
		} else {
			float angle = transform.eulerAngles.magnitude * Mathf.Deg2Rad;
			Vector2 force = new Vector2(acceleration * Mathf.Cos (angle), acceleration * Mathf.Sin (angle));
			if(transform.position.x > Roomba.instance.transform.position.x) {
				//force = force * -1;
			}
			//transform.TransformDirection(Vector3.forward);
			GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
			
			if(GetComponent<Rigidbody2D>().velocity.magnitude > maxSpeed) {
				GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, maxSpeed);
			}
		}
	}
}
