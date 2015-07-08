using UnityEngine;
using System.Collections;

public class Tank : MonoBehaviour {

	public float maxSpeed = 3f;
	public float acceleration = 3f;
	public float turnSpeed = .01f;
	public float shotTime = 5f;
	public GameObject body;
	public GameObject turret;
	public GameObject bullet;
	public GameObject crater;
	float shotCooldown;
	// Use this for initialization
	void Start () {
		shotCooldown = shotTime;
	}

	// Update is called once per frame
	void Update () {
		shotCooldown -= Time.deltaTime;
		if(shotCooldown <= 0f) {
			fireShot();
			shotCooldown = shotTime;
		}
	}

	void fireShot() {
		if(GameEngine.gameOver || GetComponentInChildren<Suckable>().beingSucked)
			return;
		Vector3 shotPoint = GetComponentInChildren<TankShotPoint>().transform.position;
		Vector3 diff = Roomba.instance.transform.position - shotPoint;
		diff.Normalize();
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

		GameObject newBullet = (GameObject)Instantiate(bullet, shotPoint, Quaternion.identity);
		Physics2D.IgnoreCollision(GetComponentInChildren<Collider2D>(), newBullet.GetComponent<Collider2D>());
		newBullet.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

		GetComponentInChildren<TankShotPoint>().flashCooldown = 4;

		GetComponent<AudioSource>().Play();
	}
	void FixedUpdate() {
		if(GameEngine.gameOver)
			return;
		if(GetComponentInChildren<Suckable>().beingSucked) {
			float suckSpeed = 1f;
			if(GetComponentInChildren<Suckable>().hitPoints <= 0f)
				suckSpeed = 10f;
			Vector2 moveVector = Vector2.MoveTowards(transform.position, Roomba.instance.transform.position, suckSpeed * Time.deltaTime);
			//float shake = .1f;
			//moveVector.x += Random.Range (-shake, shake);
			//moveVector.y += Random.Range(-shake, shake);
			transform.position = moveVector;
			//GetComponent<Rigidbody2D>().velocity = moveVector;
			float rot_z = transform.rotation.eulerAngles.z;
			rot_z += 5f;
			transform.rotation = Quaternion.Euler(0f,0f,rot_z);
		} else {
			float curRot = transform.rotation.eulerAngles.z;
			Vector3 diff = Roomba.instance.transform.position - transform.position;
			diff.Normalize();
			float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
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
			transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

			float angle = body.transform.eulerAngles.magnitude * Mathf.Deg2Rad;
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
