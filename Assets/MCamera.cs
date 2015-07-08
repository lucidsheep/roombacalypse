using UnityEngine;
using System.Collections;

public class MCamera : MonoBehaviour {

	public GameObject target;

	float shakeCooldown = 0f;
	float shakeMagnitude = 0f;

	public static MCamera instance;
	// Use this for initialization
	void Awake () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void FixedUpdate() {
		if(target != null) {
			Vector3 newPos = target.transform.position;
			newPos.z = -10;
			transform.position = newPos;
		}
		shakeCooldown -= Time.deltaTime;
		if(shakeCooldown > 0f) {
			Vector3 shakePos;
			shakePos.x = Random.Range (-shakeMagnitude, shakeMagnitude);
			shakePos.y = Random.Range (-shakeMagnitude, shakeMagnitude);
			shakePos.z = 0f;
			transform.position += shakePos;
		}
		Vector3 finalPos = transform.position;
		if(transform.position.x <= -17.5f) {
			finalPos.x = -17.5f;
		} else if(finalPos.x >= 17.5f) {
			finalPos.x = 17.5f;
		}
		if(transform.position.y <= -23f) {
			finalPos.y = -23f;
		} else if(finalPos.y >= 23f) {
			finalPos.y = 23f;
		}
		transform.position = finalPos;
	}

	public static void ScreenShake(float duration, float magnitude = 1f){
		if(duration > instance.shakeCooldown){
			instance.shakeCooldown = duration;
			instance.shakeMagnitude = magnitude;
		}
	}
}
